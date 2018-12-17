using FTN.Common;
using FTN.Services.NetworkModelService.TestClientUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelventDMS.Services.NetworkModelService.TestClient.TestsUI;

namespace NMSTestClientUI.UserControls
{
    /// <summary>
    /// Interaction logic for GetRelatedValues.xaml
    /// </summary>
    public partial class GetRelatedValues : UserControl
    {
        private TestGda tgda;
        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();
        private Dictionary<ModelCode, string> propertiesDesc = new Dictionary<ModelCode, string>();
        private HashSet<DMSType> referencedDmsTypes = new HashSet<DMSType>();

        public ObservableCollection<GlobalIdentifierViewModel> GlobalIdentifiersRelated { get; private set; }
        public ObservableCollection<PropertyViewModel> RelationalProperties { get; private set; }

        public GlobalIdentifierViewModel SelectedGID { get; set; }
        public PropertyViewModel SelectedProperty { get; set; }

        public GetRelatedValues()
        {
            InitializeComponent();
            DataContext = this;

            GlobalIdentifiersRelated = new ObservableCollection<GlobalIdentifierViewModel>();
            RelationalProperties = new ObservableCollection<PropertyViewModel>();

            try
            {
                tgda = new TestGda();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GetRelatedValues", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            foreach(DMSType dmsType in Enum.GetValues(typeof(DMSType)))
            {
                if(dmsType == DMSType.MASK_TYPE)
                {
                    continue;
                }

                ModelCode dmsTypesModelCode = modelResourcesDesc.GetModelCodeFromType(dmsType);
                tgda.GetExtentValues(dmsTypesModelCode, new List<ModelCode> { ModelCode.IDOBJ_GID }, null).ForEach(g => GlobalIdentifiersRelated.Add(new GlobalIdentifierViewModel()
                {
                    GID = g,
                    Type = dmsTypesModelCode.ToString(),
                }));
            }
        }

        private void GlobalIdentifiersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PropertiesInRelated.Children.Clear();
            RelationalProperties.Clear();
            SelectedProperty = null;
            RelatedValues.Document.Blocks.Clear();

            short type = ModelCodeHelper.ExtractTypeFromGlobalId(SelectedGID.GID);
            List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds((DMSType)type);


            foreach (ModelCode property in properties)
            {
                Property prop = new Property(property);
                if (prop.Type != PropertyType.Reference && prop.Type != PropertyType.ReferenceVector)
                {
                    continue;
                }

                RelationalProperties.Add(new PropertyViewModel() { Property = property });
            }
        }

        private void RelationalPropertiesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedProperty == null)
            {
                return;
            }

            PropertiesInRelated.Children.Clear();
            RelatedValues.Document.Blocks.Clear();
            referencedDmsTypes.Clear();

            Label label = new Label()
            {
                FontWeight = FontWeights.UltraBold,
                Content = "Properties (for classes in selected relation)",
            };
            PropertiesInRelated.Children.Add(label);

            List<long> gidReferences = new List<long>();
            ResourceDescription rd = tgda.GetValues(SelectedGID.GID, new List<ModelCode>() { SelectedProperty.Property });
            if(rd != null)
            {
                Property prop = rd.GetProperty(SelectedProperty.Property);

                if ((short)(unchecked((long)SelectedProperty.Property & (long)ModelCodeMask.MASK_ATTRIBUTE_TYPE)) == (short)PropertyType.Reference)
                {
                    gidReferences.Add(prop.AsReference());
                }
                else if ((short)(unchecked((long)SelectedProperty.Property & (long)ModelCodeMask.MASK_ATTRIBUTE_TYPE)) == (short)PropertyType.ReferenceVector)
                {
                    gidReferences.AddRange(prop.AsReferences());
                }
            }

            if (gidReferences.Count > 0 )
            {
                foreach(long gidReference in gidReferences)
                {
                    DMSType dmsType = (DMSType)ModelCodeHelper.ExtractTypeFromGlobalId(gidReference);
                    if (!referencedDmsTypes.Contains(dmsType))
                    {
                        referencedDmsTypes.Add(dmsType);
                    }
                }
            }

            HashSet<ModelCode> referencedTypeProperties = new HashSet<ModelCode>();
            if (referencedDmsTypes.Count > 0)
            {
                foreach(DMSType referencedDmsType in referencedDmsTypes)
                {
                    foreach (ModelCode propInReferencedType in modelResourcesDesc.GetAllPropertyIds(referencedDmsType))
                    {
                        if (!referencedTypeProperties.Contains(propInReferencedType))
                        {
                            referencedTypeProperties.Add(propInReferencedType);
                        }
                    }
                }
            }

            propertiesDesc.Clear();

            if(referencedTypeProperties.Count > 0)
            {
                foreach (ModelCode property in referencedTypeProperties)
                {
                    if(propertiesDesc.ContainsKey(property))
                    {
                        continue;
                    }

                    propertiesDesc.Add(property, property.ToString());

                    CheckBox checkBox = new CheckBox()
                    {
                        Content = property.ToString(),
                    };
                    PropertiesInRelated.Children.Add(checkBox);
                }
            }
        }

        private void ButtonGetRelatedValues_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedProperty == null || referencedDmsTypes == null || referencedDmsTypes.Count == 0)
            {
                return;
            }

            List<ModelCode> selectedProperties = new List<ModelCode>();

            foreach (var child in PropertiesInRelated.Children)
            {
                if (child is CheckBox checkBox)
                {
                    if (checkBox.IsChecked.Value)
                    {
                        foreach (KeyValuePair<ModelCode, string> keyValuePair in propertiesDesc)
                        {
                            if (keyValuePair.Value.Equals(checkBox.Content))
                            {
                                selectedProperties.Add(keyValuePair.Key);
                            }
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Returned entities" + Environment.NewLine + Environment.NewLine);

            try
            {
                if (referencedDmsTypes.Count == 1)
                {
                    //association: source=SelectedProperty, type=referencedDmsTypes.First()
                    Association association = new Association(SelectedProperty.Property, modelResourcesDesc.GetModelCodeFromType(referencedDmsTypes.First()));
                    List<long> gids = tgda.GetRelatedValues(SelectedGID.GID, selectedProperties, association, sb);
                }
                else
                {
                    throw new NotImplementedException("referencedDmsTypes count greater than 1");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "GetRelatedValues", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            RelatedValues.Document.Blocks.Clear();
            RelatedValues.AppendText(sb.ToString());
        }
    }
}
