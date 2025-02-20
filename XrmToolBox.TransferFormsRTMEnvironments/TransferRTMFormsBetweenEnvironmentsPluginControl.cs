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
using XrmToolBox.Extensibility;
using static System.Windows.Forms.ListViewItem;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace XrmToolBox.TransferRTMFormsBetweenEnvironments
{
    public partial class TransferRTMFormsBetweenEnvironmentsPluginControl : MultipleConnectionsPluginControlBase
    {
        private Settings mySettings;

        private List<ListViewItem> _sourceForms = new List<ListViewItem>();

        private IOrganizationService _targetService;
        private List<ListViewItem> _targetForms = new List<ListViewItem>();

        const string FORM_STATUS_UPDATED = "It will be updated";
        const string FORM_STATUS_NEW = "It will be created";
        const string FORM_STATUS_EXIST = "Exists in target";

        private enum ServiceType
        {
            Source,
            Target
        }


        public TransferRTMFormsBetweenEnvironmentsPluginControl()
        {
            InitializeComponent();
        }

        private void TransferFormsRTMEnvironmentsPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                prepare();
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
            GetForms(false);
        }

        private void prepare()
        {
            int w = (this.ParentForm.Width / 2) - 15;
            gb_SourceForms.Width = w;
            gb_TargetForms.Width = w;
            gb_environments.Width = w;
            gb_settings.Width = w;
            p_FormsSourceStatus.Width = w;
            p_FormsTargetStatus.Width = w;

            //Source forms
            int wSourceForms = ((gb_SourceForms.ClientSize.Width - GetVScrollBarWidth()) / 2) - 100;

            lv_SourceForms.Columns.Clear();
            lv_SourceForms.Columns.Add("Form Name", wSourceForms);
            lv_SourceForms.Columns.Add("Form Relation", wSourceForms);
            lv_SourceForms.Columns.Add("Form Status", 100);
            lv_SourceForms.Columns.Add("Comparer Status", 100);

            //Target forms

            int wTargetForms = ((gb_TargetForms.ClientSize.Width - GetVScrollBarWidth()) / 2) - 100;

            lv_TargetForms.Columns.Clear();
            lv_TargetForms.Columns.Add("Form Name", wTargetForms);
            lv_TargetForms.Columns.Add("Form Relation", wTargetForms);
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

        private int GetVScrollBarWidth()
        {
            return SystemInformation.VerticalScrollBarWidth;
        }

        private void GetForms(bool onlyTarget)
        {
            var loadSourceForms = true;
            var loadTargetForms = true;

            if (onlyTarget == false && lv_SourceForms != null && lv_SourceForms.Items.Count > 0)
            {            
                var openSelectedSource = MessageBox.Show("Do you want to reload the forms from the source environment?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(openSelectedSource == DialogResult.No)
                {
                    loadSourceForms = false;
                    
                }           
            }

            if (loadSourceForms)
            {
                lv_SourceForms.Items.Clear();
                _sourceForms.Clear();

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
                            List<String> formsNameRelated = new List<String>();

                            foreach (var entity in formsSource.Entities)
                            {
                                if (!formsNameRelated.Contains(entity.Attributes["msdynmkt_name"].ToString()))
                                {
                                    var formRelation = string.Empty;

                                    if (entity.Contains("msdynmkt_redirecturl") && !String.IsNullOrEmpty(entity.Attributes["msdynmkt_redirecturl"].ToString()))
                                    {
                                        var related = formsSource.Entities.Where(k => k.Id == Guid.Parse(entity.Attributes["msdynmkt_redirecturl"].ToString().Split('/').Last())).FirstOrDefault();
                                        if (related != null)
                                        {
                                            formRelation = related.Attributes["msdynmkt_name"].ToString();
                                            formsNameRelated.Add(formRelation);
                                        }
                                    }

                                    _sourceForms.Add(new ListViewItem(new string[] { entity.Attributes["msdynmkt_name"].ToString(), string.IsNullOrEmpty(formRelation) ? "" : formRelation, entity.FormattedValues["statuscode"].ToString(), FORM_STATUS_EXIST }) { ForeColor = Color.Red });
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No forms found in the source environment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        args.Result = formsSource;
                    },
                    PostWorkCallBack = (args) =>
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        var result = args.Result as EntityCollection;
                        if (result != null)
                        {
                            //lv_SourceForms.Items.AddRange(_sourceForms.OrderBy(k => k.Text).ToArray());
                            //MessageBox.Show($"Found {result.Entities.Count} forms");
                        }
                    }
                });
            }

            if (lv_TargetForms != null && lv_TargetForms.Items.Count > 0)
            {
                var openSelectedSource = MessageBox.Show("Do you want to reload the forms from the target environment?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (openSelectedSource == DialogResult.No)
                {
                    loadTargetForms = false;
                    if(_sourceForms.Count > 0)
                        lv_SourceForms.Items.AddRange(_sourceForms.OrderBy(k => k.Text).ToArray());
                }
            }

            if (_targetService != null && loadTargetForms)
            {
                lv_TargetForms.Items.Clear();
                _targetForms.Clear();

                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Getting forms from the target",
                    Work = (worker, args) =>
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
                            List<String> formsNameRelated = new List<String>();

                            foreach (var entity in formsTarget.Entities)
                            {
                                if (!formsNameRelated.Contains(entity.Attributes["msdynmkt_name"].ToString()))
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

                                    var formRelation = string.Empty;

                                    if (entity.Contains("msdynmkt_redirecturl") && !String.IsNullOrEmpty(entity.Attributes["msdynmkt_redirecturl"].ToString()))
                                    {
                                        var related = formsTarget.Entities.Where(k => k.Id == Guid.Parse(entity.Attributes["msdynmkt_redirecturl"].ToString().Split('/').Last())).FirstOrDefault();
                                        if (related != null)
                                        {
                                            formRelation = related.Attributes["msdynmkt_name"].ToString();
                                            formsNameRelated.Add(formRelation);
                                        }
                                    }

                                    _targetForms.Add(new ListViewItem(new string[] { entity.Attributes["msdynmkt_name"].ToString(), string.IsNullOrEmpty(formRelation) ? "" : formRelation, entity.FormattedValues["statuscode"].ToString(), existFormInSource ? FORM_STATUS_UPDATED : FORM_STATUS_NEW }) { ForeColor = existFormInSource ? Color.Red : Color.Green });
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No forms found in the source environment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        args.Result = formsTarget;
                    },
                    PostWorkCallBack = (args) =>
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        var result = args.Result as EntityCollection;
                        if (result != null)
                        {
                            lv_TargetForms.Items.AddRange(_targetForms.OrderBy(k => k.Text).ToArray());

                            //lv_SourceForms.Clear();
                            //Mark Form in Source if exist or new
                            foreach (var itemSource in _sourceForms)
                            {
                                var exist = _targetForms.Select(k => k.Text).ToList().Exists(k => k == itemSource.Text);
                                if (!exist)
                                {
                                    itemSource.ForeColor = Color.Green;
                                    itemSource.SubItems[2].Text = FORM_STATUS_NEW;
                                }
                            }
                            lv_SourceForms.Items.AddRange(_sourceForms.OrderBy(k => k.Text).ToArray());
                        }
                    }
                });
            }
            else
            {
                var openSelectedTarget = MessageBox.Show("Do you want to select a target environment?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (openSelectedTarget == DialogResult.Yes)
                {
                    AddAdditionalOrganization();
                    GetForms(true);
                }
            }
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
                prepare();
            }
        }

        private void lv_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(((ListView)sender).SelectedItems.Count > 0)
                ((ListViewItem)((ListView)sender).SelectedItems[0]).Selected = false;
        }

        private void TransferRTMFormsBetweenEnvironmentsPluginControl_Resize(object sender, EventArgs e)
        {
            prepare();
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
                var detail = (ConnectionDetail)e.NewItems[0];
                _targetService = detail.ServiceClient;

                l_environmentTargetValue.Text = detail.ConnectionName;
                l_environmentTargetValue.ForeColor = Color.Green;
            }
        }

        private void l_FormsSourceStatus_Click(object sender, EventArgs e)
        {

        }
    }
}