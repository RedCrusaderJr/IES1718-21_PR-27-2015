using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using FTN.Services.NetworkModelService.DataModel.Wires;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    public static class IES21Converter
    {
        #region Populate ResourceDescription

        public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
        {
            if ((cimIdentifiedObject != null) && (rd != null))
            {
                if (cimIdentifiedObject.MRIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
                }
                if (cimIdentifiedObject.NameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
                }
                if (cimIdentifiedObject.AliasNameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
                }
            }
        }
 
        public static void PopulateConnectivityNodeContainerProperties(ConnectivityNodeContainer cimConnectivityNodeContainer, ResourceDescription rd)
        {
            if((cimConnectivityNodeContainer != null) && (rd != null))
            {
                IES21Converter.PopulateIdentifiedObjectProperties(cimConnectivityNodeContainer, rd);
            }
        }
 
        public static void PopulateConnectivityNodeProperties(ConnectivityNode cimConnectivityNode, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimConnectivityNode != null) && (rd != null))
            {
                IES21Converter.PopulateIdentifiedObjectProperties(cimConnectivityNode, rd);

                if(cimConnectivityNode.DescriptionHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CONNECTNODE_DESCRIPTION, cimConnectivityNode.Description));
                }
                if(cimConnectivityNode.ConnectivityNodeContainerHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimConnectivityNode.ConnectivityNodeContainer.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimConnectivityNode.GetType().ToString()).Append(" rdfID = \"").Append(cimConnectivityNode.ID);
                        report.Report.Append("\" - Failed to set reference to ConnectivityNodeContainer: rdfID \"").Append(cimConnectivityNode.ConnectivityNodeContainer.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.CONNECTNODE_CONNECTNODECONTAINER, gid));
                }
            }
        }
  
        public static void PopulateACLineSegmentProperties(ACLineSegment cimACLineSegment, ResourceDescription rd)
        {
            if ((cimACLineSegment != null) && (rd != null))
            {
                IES21Converter.PopulateConductorProperties(cimACLineSegment, rd);

                if (cimACLineSegment.B0chHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEG_B0CH, cimACLineSegment.B0ch));
                }
                if (cimACLineSegment.BchHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEG_BCH, cimACLineSegment.Bch));
                }
                if (cimACLineSegment.G0chHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEG_G0CH, cimACLineSegment.G0ch));
                }
                if (cimACLineSegment.GchHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEG_GCH, cimACLineSegment.Gch));
                }
                if (cimACLineSegment.R0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEG_R0, cimACLineSegment.R0));
                }
                if (cimACLineSegment.RHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEG_R, cimACLineSegment.R));
                }
                if (cimACLineSegment.X0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEG_X0, cimACLineSegment.X0));
                }
                if (cimACLineSegment.XHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEG_X, cimACLineSegment.X));
                }
            }
        }
 
        public static void PopulateDCLineSegmentProperties(DCLineSegment cimDCLineSegment, ResourceDescription rd)
        {
            if ((cimDCLineSegment != null) && (rd != null))
            {
                IES21Converter.PopulateConductorProperties(cimDCLineSegment, rd);
            }
        }

        public static void PopulateSeriesCompensatorProperties(SeriesCompensator cimSeriesCompensator, ResourceDescription rd)
        {
            if ((cimSeriesCompensator != null) && (rd != null))
            {
                IES21Converter.PopulateIdentifiedObjectProperties(cimSeriesCompensator, rd);

                if(cimSeriesCompensator.R0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SERIESCOMPENSATOR_R0, cimSeriesCompensator.R0));
                }
                if (cimSeriesCompensator.RHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SERIESCOMPENSATOR_R, cimSeriesCompensator.R));
                }
                if (cimSeriesCompensator.X0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SERIESCOMPENSATOR_X0, cimSeriesCompensator.X0));
                }
                if (cimSeriesCompensator.XHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SERIESCOMPENSATOR_X, cimSeriesCompensator.X));
                }
            }
        }

        public static void PopulateConductorProperties(Conductor cimConductor, ResourceDescription rd)
        {
            if ((cimConductor != null) && (rd != null))
            {
                IES21Converter.PopulateIdentifiedObjectProperties(cimConductor, rd);

                if (cimConductor.LengthHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CONDUCTOR_LENGTH, cimConductor.Length));
                }
            }
        }

        //TODO: look again
        public static void PopulateTerminalProperties(Terminal cimTerminal, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimTerminal != null) && (rd != null))
            {
                IES21Converter.PopulateIdentifiedObjectProperties(cimTerminal, rd);

                if (cimTerminal.ConductingEquipmentHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimTerminal.ConductingEquipment.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
                        report.Report.Append("\" - Failed to set reference to ConductingEquipment: rdfID \"").Append(cimTerminal.ConductingEquipment.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TERMINAL_CONDEQUIP, gid));
                }
                if (cimTerminal.ConnectivityNodeHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimTerminal.ConnectivityNode.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
                        report.Report.Append("\" - Failed to set reference to ConnectivityNode: rdfID \"").Append(cimTerminal.ConnectivityNode.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TERMINAL_CONNECTNODE, gid));
                }
            }
        }
        #endregion Populate ResourceDescription
    }
}
