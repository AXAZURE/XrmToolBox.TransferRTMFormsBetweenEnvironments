using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Workflow.Activities;
using XrmToolBox.Extensibility;
using static System.Windows.Forms.ListViewItem;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TransferFormsRTMEnvironments
{
    public partial class TransferRTMFormsBetweenEnvironmentsPluginControl : MultipleConnectionsPluginControlBase
    {
        private Settings mySettings;

        private List<ListViewItem> _sourceForms = new List<ListViewItem>();
        private Dictionary<Entity, Entity> _entitiesSourceForms = new Dictionary<Entity, Entity>();

        private IOrganizationService _targetService;
        private ConnectionDetail _targetConnectionDetail;
        private List<ListViewItem> _targetForms = new List<ListViewItem>();
        private Dictionary<Entity, Entity> _entitiesTargetForms = new Dictionary<Entity, Entity>();


        const string FORM_STATUS_UPDATED = "It will be updated";
        const string FORM_STATUS_NEW = "It will be created";
        const string FORM_STATUS_NOTCONNECT_TARGET = "Target pending";

        const string DONATION_BTN_ID = "HDBPHYMTQMUUA";

        public TransferRTMFormsBetweenEnvironmentsPluginControl()
        {
            InitializeComponent();
        }

        private void TransferFormsRTMEnvironmentsPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("If you detect any error or problem in the tool, please let us know so we can resolve it as soon as possible.", new Uri("https://github.com/AXAZURE/XrmToolBox.TransferRTMFormsBetweenEnvironments/issues"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                Prepare();
                //LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                //LogInfo("Settings found and loaded");
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }


        private void bt_LoadForms_Click(object sender, EventArgs e)
        {
            GetForms(false, true);
        }

        private void Prepare()
        {
            int w = (this.ParentForm.Width / 2) - 15;
            gb_SourceForms.Width = w;
            gb_TargetForms.Width = w;
            gb_environments.Width = w;
            gb_settings.Width = w;

            //Source forms
            int wSourceForms = ((gb_SourceForms.ClientSize.Width - GetVScrollBarWidth()) / 2) - 280;

            lv_SourceForms.Columns.Clear();
            lv_SourceForms.Columns.Add("Form Name (Cross-environment form key field)", wSourceForms);
            lv_SourceForms.Columns.Add("Preview form", 80);
            lv_SourceForms.Columns.Add("Form Redirect", wSourceForms);
            lv_SourceForms.Columns.Add("Preview form redirect", 100);
            lv_SourceForms.Columns.Add("Form Status", 100);
            lv_SourceForms.Columns.Add("Comparer Status", 100);
            

            //Target forms
            int wTargetForms = ((gb_TargetForms.ClientSize.Width - GetVScrollBarWidth()) / 2) - 280;

            lv_TargetForms.Columns.Clear();
            lv_TargetForms.Columns.Add("Form Name Cross-environment form key field)", wTargetForms);
            lv_TargetForms.Columns.Add("Preview form", 80);
            lv_TargetForms.Columns.Add("Form Redirect", wTargetForms);
            lv_TargetForms.Columns.Add("Preview form redirect", 100);
            lv_TargetForms.Columns.Add("Form Status", 100);
            lv_TargetForms.Columns.Add("Transfer Status", 100);

            //Enviroments
            if (ConnectionDetail != null)
            {
                l_environmentSourceValue.Text = ConnectionDetail.ConnectionName;
            }
            else
            {
                l_environmentSourceValue.Text = "Pending selected";
                l_environmentSourceValue.ForeColor = Color.Red;
            }
        }

        private void SummaryStatus()
        {
            if (lv_SourceForms.Items.Count > 0)
            {
                var formsSourceSelected = lv_SourceForms.Items.Cast<ListViewItem>().Where(k => k != null && k.Checked).ToList();

                //Forms Source
                l_FormsSourceStatus.Text = $"Comparer status forms : Source: {lv_SourceForms.Items.Count}";

                if (_targetForms != null && _targetForms.Count > 0)
                {
                    l_FormsSourceStatus.Text += $" - Target: {_targetForms.Count}";
                    p_FormsSourceStatus.BackColor = Color.Yellow;
                }
                else
                {
                    p_FormsSourceStatus.BackColor = Color.White;
                }

                l_FormsSourceStatus.Text += $" || Will be created: {formsSourceSelected.Select(k => k.SubItems[5]).Where(s => s.Text == FORM_STATUS_NEW).Count()} - Will be Updated: {formsSourceSelected.Select(k => k.SubItems[5]).Where(s => s.Text == FORM_STATUS_UPDATED).Count()}";

                if (formsSourceSelected.Count > 0)
                    bt_TransferForms.Enabled = true;
                else
                    bt_TransferForms.Enabled = false;
            }
        }

        private int GetVScrollBarWidth()
        {
            return SystemInformation.VerticalScrollBarWidth;
        }

        private void GetForms(bool onlyTarget, bool showMessagesReload)
        {
            var loadSourceForms = true;
            var loadTargetForms = true;

            if (onlyTarget == false && lv_SourceForms != null && lv_SourceForms.Items.Count > 0 && showMessagesReload)
            {            
                var openSelectedSource = MessageBox.Show("Do you want to reload the forms from the source environment?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(openSelectedSource == DialogResult.No)
                {
                    loadSourceForms = false;                    
                }           
            }

            if (Service != null && loadSourceForms)
            {
                _sourceForms.Clear();
                _entitiesSourceForms.Clear();

                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Getting forms from the source",
                    Work = (worker, args) =>
                    {
                        EntityCollection formsSource = Service.RetrieveMultiple(new QueryExpression("msdynmkt_marketingform")
                        {
                            ColumnSet = new ColumnSet(true),
                            Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression("ismanaged", ConditionOperator.Equal, false)
                                }
                            },
                            Orders =
                            {
                                new OrderExpression("msdynmkt_redirecturl", OrderType.Descending)
                            }
                        });


                        if (formsSource != null && formsSource.Entities != null && formsSource.Entities.Count > 0)
                        {
                            List<String> formsNameRedirect = new List<String>();
                            var count = 0;
                            foreach (var entity in formsSource.Entities)
                            {
                                if (!formsNameRedirect.Contains(entity.Attributes["msdynmkt_name"].ToString()))
                                {
                                    var formRedirect = string.Empty;

                                    if (entity.Contains("msdynmkt_redirecturl") && !String.IsNullOrEmpty(entity.Attributes["msdynmkt_redirecturl"].ToString()))
                                    {
                                        var related = formsSource.Entities.Where(k => k.Id == Guid.Parse(entity.Attributes["msdynmkt_redirecturl"].ToString().Split('/').Last())).FirstOrDefault();
                                        if (related != null)
                                        {
                                            formRedirect = related.Attributes["msdynmkt_name"].ToString();
                                            formsNameRedirect.Add(formRedirect);
                                            _entitiesSourceForms.Add(entity, related);
                                        }
                                    }
                                    else
                                    {
                                        _entitiesSourceForms.Add(entity, null);
                                    }
                                                                        
                                    _sourceForms.Add(new ListViewItem(new string[] { entity.Attributes["msdynmkt_name"].ToString(), "", string.IsNullOrEmpty(formRedirect) ? "" : formRedirect, "", entity.FormattedValues["statuscode"].ToString(), FORM_STATUS_NOTCONNECT_TARGET}) { ForeColor = Color.Red, ImageIndex = 0 });
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No forms found in the source environment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        args.Result = _sourceForms;
                    },
                    PostWorkCallBack = (args) =>
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        if (lv_TargetForms != null && lv_TargetForms.Items.Count > 0 && showMessagesReload)
                        {
                            var openSelectedSource = MessageBox.Show("Do you want to reload the forms from the target environment?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (openSelectedSource == DialogResult.No)
                            {
                                loadTargetForms = false;
                                if (_sourceForms.Count > 0)
                                {
                                    lv_SourceForms.Items.Clear();
                                    lv_SourceForms.Items.AddRange(_sourceForms.OrderBy(k => k.Text).ToArray());
                                    lv_SourceForms.Items.Cast<ListViewItem>().All(k => k.Checked = true);
                                }
                            }
                        }

                        if (_targetService != null && loadTargetForms)
                        {
                            _targetForms.Clear();
                            _entitiesTargetForms.Clear();

                            WorkAsync(new WorkAsyncInfo
                            {
                                Message = "Getting forms from the target",
                                Work = (worker, argsTarget) =>
                                {
                                    EntityCollection formsTarget = _targetService.RetrieveMultiple(new QueryExpression("msdynmkt_marketingform")
                                    {
                                        ColumnSet = new ColumnSet(true),
                                        Criteria =
                            {
                                Conditions =
                                {
                                    new ConditionExpression("ismanaged", ConditionOperator.Equal, false)
                                }
                            },
                                        Orders =
                            {
                                new OrderExpression("msdynmkt_redirecturl", OrderType.Descending)
                            }
                                    });


                                    if (formsTarget != null && formsTarget.Entities != null && formsTarget.Entities.Count > 0)
                                    {
                                        List<String> formsNameRedirect = new List<String>();

                                        foreach (var entity in formsTarget.Entities)
                                        {
                                            if (!formsNameRedirect.Contains(entity.Attributes["msdynmkt_name"].ToString()))
                                            {
                                                var existFormInSource = _sourceForms.Exists(k => k.Text == entity.Attributes["msdynmkt_name"].ToString());
                                                if (!existFormInSource)
                                                {
                                                    foreach (var formInSource in _sourceForms)
                                                    {
                                                        var subItems = formInSource.SubItems;
                                                        foreach (ListViewSubItem subItem in subItems)
                                                        {
                                                            if (subItem.Text == entity.Attributes["msdynmkt_name"].ToString())
                                                            {
                                                                existFormInSource = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                                var formRedirect = string.Empty;

                                                if (entity.Contains("msdynmkt_redirecturl") && !String.IsNullOrEmpty(entity.Attributes["msdynmkt_redirecturl"].ToString()))
                                                {
                                                    var related = formsTarget.Entities.Where(k => k.Id == Guid.Parse(entity.Attributes["msdynmkt_redirecturl"].ToString().Split('/').Last())).FirstOrDefault();
                                                    if (related != null)
                                                    {
                                                        formRedirect = related.Attributes["msdynmkt_name"].ToString();
                                                        formsNameRedirect.Add(formRedirect);
                                                        _entitiesTargetForms.Add(entity, related);
                                                    }
                                                }
                                                else
                                                {
                                                    _entitiesTargetForms.Add(entity, null);
                                                }

                                                _targetForms.Add(new ListViewItem(new string[] { entity.Attributes["msdynmkt_name"].ToString(), "", string.IsNullOrEmpty(formRedirect) ? "" : formRedirect, "", entity.FormattedValues["statuscode"].ToString(), existFormInSource ? FORM_STATUS_UPDATED : FORM_STATUS_NEW }) { ForeColor = existFormInSource ? Color.Red : Color.Green, ImageIndex = 0 });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No forms found in the source environment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                    argsTarget.Result = _targetForms;
                                },
                                PostWorkCallBack = (argsTarget) =>
                                {
                                    if (argsTarget.Error != null)
                                    {
                                        MessageBox.Show(argsTarget.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                    SetComparerFormsSourceFromTarget();
                                }
                            });
                        }
                        else
                        {
                            if (loadTargetForms)
                            {
                                var openSelectedTarget = MessageBox.Show("Do you want to select a target environment?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (openSelectedTarget == DialogResult.Yes)
                                {
                                    AddAdditionalOrganization();
                                    GetForms(false, false);
                                }
                            }

                            SetComparerFormsSourceFromTarget();
                        }
                    }
                });
            }
        }

        private void SetComparerFormsSourceFromTarget()
        {
            //Target
            if (_targetForms != null && _targetForms.Count > 0)
            {
                lv_TargetForms.Items.Clear();

                bt_TransferForms.Enabled = true;

                lv_TargetForms.Items.AddRange(_targetForms.OrderBy(k => k.Text).ToArray());

                //Mark Form in Source if exist or new
                foreach (var itemSource in _sourceForms)
                {
                    var exist = _targetForms.Select(k => k.Text).ToList().Exists(k => k == itemSource.Text);
                    if (!exist)
                    {
                        itemSource.ForeColor = Color.Green;
                        itemSource.SubItems[5].Text = FORM_STATUS_NEW;
                    }
                    else
                    {
                        itemSource.ForeColor = Color.Black;
                        itemSource.SubItems[5].Text = FORM_STATUS_UPDATED;
                    }
                }
                lv_SourceForms.Items.Clear();
                lv_SourceForms.Items.AddRange(_sourceForms.OrderBy(k => k.Text).ToArray());
                lv_SourceForms.Items.Cast<ListViewItem>().All(k => k.Checked = true);
            }
            else
            {
                //Source
                //Reset color in Forms items in Source without Target
                if (_sourceForms.Count > 0)
                {
                    foreach (var itemSource in _sourceForms)
                    {
                        itemSource.ForeColor = Color.Black;
                    }

                    lv_SourceForms.Items.Clear();
                    lv_SourceForms.Items.AddRange(_sourceForms.OrderBy(k => k.Text).ToArray());
                    lv_SourceForms.Items.Cast<ListViewItem>().All(k => k.Checked = true);
                }

                bt_TransferForms.Enabled = false;
            }

            SummaryStatus();
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransferFormsRTMEnvironmentsPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
                l_environmentSourceValue.ForeColor = Color.Green;
                Prepare();
            }
        }

        private void lv_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(((ListView)sender).SelectedItems.Count > 0)
                ((ListViewItem)((ListView)sender).SelectedItems[0]).Selected = false;
        }

        private void TransferRTMFormsBetweenEnvironmentsPluginControl_Resize(object sender, EventArgs e)
        {
            Prepare();
        }

        private void bt_SelectTarget_Click(object sender, EventArgs e)
        {
            AddAdditionalOrganization();
        }

        protected override void ConnectionDetailsUpdated(NotifyCollectionChangedEventArgs e)
        {
            // For now, only support one target org
            if (e.Action.Equals(NotifyCollectionChangedAction.Add))
            {
                _targetConnectionDetail = (ConnectionDetail)e.NewItems[0];
                _targetService = _targetConnectionDetail.ServiceClient;

                l_environmentTargetValue.Text = _targetConnectionDetail.ConnectionName;
                l_environmentTargetValue.ForeColor = Color.Green;
            }
        }

        private void bt_Donate_Click(object sender, EventArgs e)
        {
            var url = string.Format("https://www.paypal.com/donate/?hosted_button_id={0}", DONATION_BTN_ID);
            Process.Start(url);
        }

        private void bt_TransferForms_Click(object sender, EventArgs e)
        {
            var formsSourceSelected = lv_SourceForms.Items.Cast<ListViewItem>().Where(k => k.Checked).ToList();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Transfer forms selected",
                Work = (worker, args) =>
                {
                    try
                    {
                        foreach (var formSelected in formsSourceSelected)
                        {
                            var entitySourceForm = new Entity("msdynmkt_marketingform");
                            var entityTargetForm = new Entity("msdynmkt_marketingform");
                            var entitySourceRedirectForm = new Entity("msdynmkt_marketingform");
                            var entityTargetRedirectForm = new Entity("msdynmkt_marketingform");

                            if (cb_TransfersFormsRedirect.Checked)
                            {
                                entitySourceForm = _entitiesSourceForms.Select(k => k.Key).Where(k => k.Attributes["msdynmkt_name"].ToString() == formSelected.Text).FirstOrDefault();
                                entitySourceRedirectForm = _entitiesSourceForms.Where(k => k.Value != null).Select(k => k.Value).Where(k => k.Attributes["msdynmkt_name"].ToString() == formSelected.SubItems[1].Text).FirstOrDefault();
                                entityTargetForm = _entitiesTargetForms.Select(k => k.Key).Where(k => k.Attributes["msdynmkt_name"].ToString() == formSelected.Text).FirstOrDefault();
                                entityTargetRedirectForm = _entitiesTargetForms.Where(k => k.Value != null).Select(k => k.Value).Where(k => k.Attributes["msdynmkt_name"].ToString() == formSelected.SubItems[1].Text).FirstOrDefault();
                            }
                            else
                            {
                                entitySourceForm = _entitiesSourceForms.Select(k => k.Key).Where(k => k.Attributes["msdynmkt_name"].ToString() == formSelected.Text).FirstOrDefault();
                                entitySourceRedirectForm = null;
                                entityTargetForm = _entitiesTargetForms.Select(k => k.Key).Where(k => k.Attributes["msdynmkt_name"].ToString() == formSelected.Text).FirstOrDefault();
                                entityTargetRedirectForm = null;
                            }
                            TransferForm(entitySourceForm, entityTargetForm, entitySourceRedirectForm, entityTargetRedirectForm);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }                    

                    args.Result = formsSourceSelected;
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show("An error occurred while transferring the selected forms to the target environment, check the logs to see the problem.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"The selected forms have been successfully transferred to the target environment", "Transfer of forms", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        var openSelectedSource = MessageBox.Show("Do you want to reload the forms from the source and target environment?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (openSelectedSource == DialogResult.Yes)
                        {
                            GetForms(false, false);
                        }
                    }                
                }
            });
        }

        private Boolean TransferForm(Entity formSource, Entity formTarget, Entity formSourceRedirect, Entity formTargetRedirect)
        {
            Guid recordCrearteOrUpdate = Guid.Empty;

            try
            {
                //Update form redirect in target
                if (formSourceRedirect != null && formTargetRedirect != null)
                {
                    try
                    {
                        formSourceRedirect.Id = formTargetRedirect.Id;
                        formSourceRedirect.Attributes["msdynmkt_marketingformid"] = formTargetRedirect.Id;
                        formSourceRedirect["statuscode"] = cb_TransfersFormsInDraft.Checked ? new OptionSetValue(1) : formSourceRedirect["statuscode"];

                        _targetService.Update(formSourceRedirect);
                        recordCrearteOrUpdate = formSourceRedirect.Id;
                    }
                    catch (Exception ex)
                    {
                        LogError(string.Format("TransferForm Update Form Target Redirect Error : {0}", ex.Message));
                    }
                }
                else if (formSourceRedirect != null && formTargetRedirect == null)
                {
                    //Create form redirect in target
                    try
                    {
                        formSourceRedirect.Id = Guid.NewGuid();
                        formSourceRedirect.Attributes["msdynmkt_marketingformid"] = formSourceRedirect.Id;
                        formSourceRedirect["statuscode"] = cb_TransfersFormsInDraft.Checked ? new OptionSetValue(1) : formSourceRedirect["statuscode"];
                        recordCrearteOrUpdate = _targetService.Create(formSourceRedirect);
                    }
                    catch (Exception ex)
                    {
                        LogError(string.Format("TransferForm Create Form Source Redirect Error : {0}", ex.Message));
                    }
                }

                //Update form source in target
                if (formSource != null && formTarget != null)
                {
                    try
                    {
                        formSource.Id = formTarget.Id;
                        formSource.Attributes["msdynmkt_marketingformid"] = formTarget.Id;
                        formSource["statuscode"] = cb_TransfersFormsInDraft.Checked ? new OptionSetValue(1) : formSource["statuscode"];

                        //Associate form redirect previously created or updated
                        if (recordCrearteOrUpdate != Guid.Empty)
                        {
                            formSource["msdynmkt_redirecturl"] = formSource.Attributes["msdynmkt_redirecturl"].ToString().Replace(formSource.Attributes["msdynmkt_redirecturl"].ToString().Split('/').Last(), recordCrearteOrUpdate.ToString());
                        }
                        else if (formTarget.Contains("msdynmkt_redirecturl"))
                        {
                            formSource["msdynmkt_redirecturl"] = formTarget.Attributes["msdynmkt_redirecturl"];
                        }

                        _targetService.Update(formSource);
                        recordCrearteOrUpdate = formSource.Id;
                    }
                    catch (Exception ex)
                    {
                        LogError(string.Format("TransferForm Update Form Target Error : {0}", ex.Message));
                    }
                }
                else if (formSource != null && formTarget == null)
                {
                    //Create form source in target
                    try
                    {
                        formSource.Id = Guid.NewGuid();
                        formSource.Attributes["msdynmkt_marketingformid"] = formSource.Id;
                        formSource["statuscode"] = cb_TransfersFormsInDraft.Checked ? new OptionSetValue(1) : formSource["statuscode"];

                        //Associate form redirect previously created or updated
                        if (recordCrearteOrUpdate != Guid.Empty)
                        {
                            formSource["msdynmkt_redirecturl"] = formSource.Attributes["msdynmkt_redirecturl"].ToString().Replace(formSource.Attributes["msdynmkt_redirecturl"].ToString().Split('/').Last(), recordCrearteOrUpdate.ToString());
                        }

                        recordCrearteOrUpdate = _targetService.Create(formSource);
                    }
                    catch (Exception ex)
                    {
                        LogError(string.Format("TransferForm Create Form Source Error : {0}", ex.Message));
                    }
                }

            }
            catch (Exception ex)
            {
                LogError(string.Format("TransferForm Error : {0}", ex.Message));
            }

            if (formSource != null)
                LogInfo(string.Format("TransferForm : Form {0} {1} in Target", formSource.Attributes["msdynmkt_name"].ToString(), recordCrearteOrUpdate != Guid.Empty ? "created/updated" : "not created/updated"));
            if(formSourceRedirect != null)
                LogInfo(string.Format("TransferForm : Form Redirect {0} {1} in Target", formSourceRedirect.Attributes["msdynmkt_name"].ToString(), recordCrearteOrUpdate != Guid.Empty ? "created/updated" : "not created/updated"));

            return recordCrearteOrUpdate != Guid.Empty;
        }

        private void lv_SourceForms_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            SummaryStatus();
        }

        private void lv_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lv_ShowFormIcon_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 3) // Índice de la columna donde quieres mostrar la imagen
            {
                
                if (e.ColumnIndex == 3 && string.IsNullOrEmpty(e.Item.SubItems[2].Text))
                {
                    e.DrawDefault = true;
                }
                else
                {
                    Image image = imgList.Images[e.Item.ImageIndex];
                    int x = e.Bounds.Left + (e.Bounds.Width - image.Width) / 2;
                    int y = e.Bounds.Top + (e.Bounds.Height - image.Height) / 2;
                    e.Graphics.DrawImage(image, x, y);

                    e.SubItem.Tag = new Rectangle(x, y, image.Width, image.Height);
                }               
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void lv_SourceForms_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = lv_SourceForms.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                ListViewHitTestInfo hitTest = lv_SourceForms.HitTest(e.X, e.Y);
                int index = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
                if (hitTest.Item != null && hitTest.SubItem != null && hitTest.SubItem.Tag != null && (index == 1 || index == 3))
                {
                    Rectangle rect = (Rectangle)hitTest.SubItem.Tag;
                    if (rect.Contains(e.Location))
                    {
                        ShowFormSource(item, index);
                    }
                }
            }
        }

        private void lv_SourceForms_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem item = lv_SourceForms.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                ListViewHitTestInfo hitTest = lv_SourceForms.HitTest(e.X, e.Y);
                if (hitTest.Item != null && hitTest.SubItem != null && hitTest.SubItem.Tag != null && (hitTest.Item.SubItems.IndexOf(hitTest.SubItem) == 1 || hitTest.Item.SubItems.IndexOf(hitTest.SubItem) == 3))
                {
                    Rectangle rect = (Rectangle)hitTest.SubItem.Tag;
                    if (rect.Contains(e.Location))
                    {
                        lv_SourceForms.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        lv_SourceForms.Cursor = Cursors.Default;
                    }
                }
                else
                {
                    lv_SourceForms.Cursor = Cursors.Default;
                }
            }
            else
            {
                lv_SourceForms.Cursor = Cursors.Default;
            }
        }

        private void ShowFormSource(ListViewItem item, int index)
        {
            var formName = index == 1 ? item.Text : item.SubItems[2].Text;
            var urlEnviroment = ConnectionDetail.WebApplicationUrl;
            var formId = _entitiesSourceForms.Where(k => index == 1 ? (k.Key.Attributes["msdynmkt_name"].ToString() == formName) : (k.Value != null && k.Value.Attributes["msdynmkt_name"].ToString() == formName)).Select(k => index == 1 ? k.Key.Id : k.Value.Id).FirstOrDefault();               

            if (formId != null) {
                

                var openSelectedTarget = MessageBox.Show("A tab of the form will open in your browser. You will need to be logged into the form environment to view it. Do you want to continue?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (openSelectedTarget == DialogResult.Yes)
                {
                    var urlForm = $"{urlEnviroment}/main.aspx?etn=msdynmkt_marketingform&pagetype=entityrecord&forceUCI=1&navbar=off&id={formId}";
                    Process.Start(urlForm);
                }                
            } 
        }

        private void lv_TargetForms_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = lv_TargetForms.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                ListViewHitTestInfo hitTest = lv_TargetForms.HitTest(e.X, e.Y);
                int index = hitTest.Item.SubItems.IndexOf(hitTest.SubItem);
                if (hitTest.Item != null && hitTest.SubItem != null && hitTest.SubItem.Tag != null && (index == 1 || index == 3))
                {
                    Rectangle rect = (Rectangle)hitTest.SubItem.Tag;
                    if (rect.Contains(e.Location))
                    {
                        ShowFormTarget(item, index);
                    }
                }
            }
        }

        private void lv_TargetForms_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem item = lv_TargetForms.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                ListViewHitTestInfo hitTest = lv_TargetForms.HitTest(e.X, e.Y);
                if (hitTest.Item != null && hitTest.SubItem != null && hitTest.SubItem.Tag != null && (hitTest.Item.SubItems.IndexOf(hitTest.SubItem) == 1 || hitTest.Item.SubItems.IndexOf(hitTest.SubItem) == 3))
                {
                    Rectangle rect = (Rectangle)hitTest.SubItem.Tag;
                    if (rect.Contains(e.Location))
                    {
                        lv_TargetForms.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        lv_TargetForms.Cursor = Cursors.Default;
                    }
                }
                else
                {
                    lv_TargetForms.Cursor = Cursors.Default;
                }
            }
            else
            {
                lv_TargetForms.Cursor = Cursors.Default;
            }
        }

        private void ShowFormTarget(ListViewItem item, int index)
        {
            var formName = index == 1 ? item.Text : item.SubItems[2].Text;
            var urlEnviroment = _targetConnectionDetail.WebApplicationUrl;
            var formId = _entitiesTargetForms.Where(k => index == 1 ? (k.Key.Attributes["msdynmkt_name"].ToString() == formName) : (k.Value != null && k.Value.Attributes["msdynmkt_name"].ToString() == formName)).Select(k => index == 1 ? k.Key.Id : k.Value.Id).FirstOrDefault();

            if (formId != null)
            {


                var openSelectedTarget = MessageBox.Show("A tab of the form will open in your browser. You will need to be logged into the form environment to view it. Do you want to continue?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (openSelectedTarget == DialogResult.Yes)
                {
                    var urlForm = $"{urlEnviroment}/main.aspx?etn=msdynmkt_marketingform&pagetype=entityrecord&forceUCI=1&navbar=off&id={formId}";
                    Process.Start(urlForm);
                }
            }
        }
    }
}