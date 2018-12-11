using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;
//TODO: videti oko namespace-ova...
using FTN.Services.NetworkModelService.DataModel.Core;
using FTN.Services.NetworkModelService.DataModel.Wires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	public class IES21Importer
	{
		/// <summary> Singleton </summary>
		private static IES21Importer ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static IES21Importer Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new IES21Importer();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		//TODO: CEMU OVO?
		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

			//// import all concrete model types (DMSType enum)
			ImportConnectivityNodeContainers();
			ImportConnectivityNodes();
			ImportACLineSegments();
			ImportDCLineSegments();
			ImportSeriesCompensators();
			ImportTerminals();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

		private void ImportConnectivityNodeContainers()
		{
			//TODO: type + cim.ID
			SortedDictionary<string, object> cimConnectivityNodeContainers = concreteModel.GetAllObjectsOfType(/*"FTN.BaseVoltage"*/"");
			if(cimConnectivityNodeContainers != null)
			{
				foreach(KeyValuePair<string,object> cimConnectivityNodeContainerPair in cimConnectivityNodeContainers)
				{
					ConnectivityNodeContainer cimConnectivityNodeContainer = cimConnectivityNodeContainerPair.Value as ConnectivityNodeContainer;

					ResourceDescription rd = CreateConnectivityNodeContainerResourceDescription(cimConnectivityNodeContainer);
					if(rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("ConnectivityNodeContainer ID = ").Append(/*cimConnectivityNodeContainer.ID*/"").Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("ConnectivityNodeContainer ID = ").Append(/*cimConnectivityNodeContainer.ID*/"").AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateConnectivityNodeContainerResourceDescription(ConnectivityNodeContainer cimConnectivityNodeContainer)
		{
			ResourceDescription rd = null;
			if(cimConnectivityNodeContainer != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CONNECTNODECONTAINER, importHelper.CheckOutIndexForDMSType(DMSType.CONNECTNODECONTAINER));
				rd = new ResourceDescription(gid);
				//TODO:  cim.ID
				importHelper.DefineIDMapping(/*cimConnectivityNodeContainer.GlobalId*/"", gid);

				IES21Converter.PopulateConnectivityNodeContainerProperties(cimConnectivityNodeContainer, rd);
			}
			return rd;
		}

		private void ImportConnectivityNodes()
		{
			//TODO: type + cim.ID
			SortedDictionary<string, object> cimConnectivityNodes = concreteModel.GetAllObjectsOfType(/*"FTN.BaseVoltage"*/"");
			if (cimConnectivityNodes != null)
			{
				foreach (KeyValuePair<string, object> cimConnectivityNodePair in cimConnectivityNodes)
				{
					ConnectivityNode cimConnectivityNode = cimConnectivityNodePair.Value as ConnectivityNode;

					ResourceDescription rd = CreateConnectivityNodeResourceDescription(cimConnectivityNode);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("ConnectivityNode ID = ").Append(/*cimConnectivityNodePair.ID*/"").Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("ConnectivityNode ID = ").Append(/*cimConnectivityNodePair.ID*/"").AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateConnectivityNodeResourceDescription(ConnectivityNode cimConnectivityNode)
		{
			ResourceDescription rd = null;
			if (cimConnectivityNode != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CONNECTNODE, importHelper.CheckOutIndexForDMSType(DMSType.CONNECTNODE));
				rd = new ResourceDescription(gid);
				//TODO:  cim.ID
				importHelper.DefineIDMapping(/*cimConnectivityNode.GlobalId*/"", gid);

				IES21Converter.PopulateConnectivityNodeProperties(cimConnectivityNode, rd);
			}
			return rd;
		}

		private void ImportACLineSegments()
		{
			//TODO: type + cim.ID
			SortedDictionary<string, object> cimACLineSegments = concreteModel.GetAllObjectsOfType(/*"FTN.BaseVoltage"*/"");
			if (cimACLineSegments != null)
			{
				foreach (KeyValuePair<string, object> cimACLineSegmentPair in cimACLineSegments)
				{
					ACLineSegment cimACLineSegment = cimACLineSegmentPair.Value as ACLineSegment;

					ResourceDescription rd = CreateACLineSegmentResourceDescription(cimACLineSegment);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("ACLineSegment ID = ").Append(/*cimACLineSegment.ID*/"").Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("ACLineSegment ID = ").Append(/*cimACLineSegment.ID*/"").AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateACLineSegmentResourceDescription(ACLineSegment cimACLineSegment)
		{
			ResourceDescription rd = null;
			if (cimACLineSegment != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ACLINESEG, importHelper.CheckOutIndexForDMSType(DMSType.ACLINESEG));
				rd = new ResourceDescription(gid);
				//TODO:  cim.ID
				importHelper.DefineIDMapping(/*cimACLineSegment.GlobalId*/"", gid);

				IES21Converter.PopulateACLineSegmentProperties(cimACLineSegment, rd);
			}
			return rd;
		}

		private void ImportDCLineSegments()
		{
			//TODO: type + cim.ID
			SortedDictionary<string, object> cimDCLineSegments = concreteModel.GetAllObjectsOfType(/*"FTN.BaseVoltage"*/"");
			if (cimDCLineSegments != null)
			{
				foreach (KeyValuePair<string, object> cimDCLineSegmentPair in cimDCLineSegments)
				{
					DCLineSegment cimDCLineSegment = cimDCLineSegmentPair.Value as DCLineSegment;

					ResourceDescription rd = CreateDCLineSegmentResourceDescription(cimDCLineSegment);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("DCLineSegment ID = ").Append(/*cimDCLineSegment.ID*/"").Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("DCLineSegment ID = ").Append(/*cimDCLineSegment.ID*/"").AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateDCLineSegmentResourceDescription(DCLineSegment cimDCLineSegment)
		{
			ResourceDescription rd = null;
			if (cimDCLineSegment != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DCLINESEG, importHelper.CheckOutIndexForDMSType(DMSType.DCLINESEG));
				rd = new ResourceDescription(gid);
				//TODO:  cim.ID
				importHelper.DefineIDMapping(/*cimDCLineSegment.GlobalId*/"", gid);

				IES21Converter.PopulateDCLineSegmentProperties(cimDCLineSegment, rd);
			}
			return rd;
		}

		private void ImportSeriesCompensators()
		{
			//TODO: type + cim.ID
			SortedDictionary<string, object> cimSeriesCompensators = concreteModel.GetAllObjectsOfType(/*"FTN.BaseVoltage"*/"");
			if (cimSeriesCompensators != null)
			{
				foreach (KeyValuePair<string, object> cimSeriesCompensatorPair in cimSeriesCompensators)
				{
					SeriesCompensator cimSeriesCompensator = cimSeriesCompensatorPair.Value as SeriesCompensator;

					ResourceDescription rd = CreateSeriesCompensatorResourceDescription(cimSeriesCompensator);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("SeriesCompensator ID = ").Append(/*cimSeriesCompensator.ID*/"").Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("SeriesCompensator ID = ").Append(/*cimSeriesCompensator.ID*/"").AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateSeriesCompensatorResourceDescription(SeriesCompensator cimSeriesCompensator)
		{
			ResourceDescription rd = null;
			if (cimSeriesCompensator != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SERIESCOMPENSATOR, importHelper.CheckOutIndexForDMSType(DMSType.SERIESCOMPENSATOR));
				rd = new ResourceDescription(gid);
				//TODO:  cim.ID
				importHelper.DefineIDMapping(/*cimSeriesCompensator.GlobalId*/"", gid);

				IES21Converter.PopulateSeriesCompensatorProperties(cimSeriesCompensator, rd);
			}
			return rd;
		}

		private void ImportTerminals()
		{
			//TODO: type + cim.ID
			SortedDictionary<string, object> cimTerminals = concreteModel.GetAllObjectsOfType(/*"FTN.BaseVoltage"*/"");
			if (cimTerminals != null)
			{
				foreach (KeyValuePair<string, object> cimTerminalPair in cimTerminals)
				{
					Terminal cimTerminal = cimTerminalPair.Value as Terminal;

					ResourceDescription rd = CreateTerminalResourceDescription(cimTerminal);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Terminal ID = ").Append(/*cimTerminal.ID*/"").Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Terminal ID = ").Append(/*cimTerminal.ID*/"").AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateTerminalsResourceDescription(Terminal cimTerminal)
		{
			ResourceDescription rd = null;
			if (cimTerminal != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TERMINAL, importHelper.CheckOutIndexForDMSType(DMSType.TERMINAL));
				rd = new ResourceDescription(gid);
				//TODO:  cim.ID
				importHelper.DefineIDMapping(/*cimTerminal.GlobalId*/"", gid);

				IES21Converter.PopulateTerminalProperties(cimTerminal, rd);
			}
			return rd;
		}

	}
}
