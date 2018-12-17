using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
	public class ConnectivityNodeContainer : PowerSystemResource
	{
		private List<long> connectivityNodes = new List<long>();
		public ConnectivityNodeContainer(long globalId)
			: base(globalId)
		{
		}

		public List<long> Terminals
		{
			get { return connectivityNodes; }
			set { connectivityNodes = value; }
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				ConnectivityNodeContainer cnc = (ConnectivityNodeContainer)obj;
				return CompareHelper.CompareLists(cnc.connectivityNodes, this.connectivityNodes);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#region IAccess implementation

		public override bool HasProperty(ModelCode property)
		{
			switch (property)
			{
				case ModelCode.CONNECTNODECONTAINER_CONNECTNODES:
					return true;

				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.CONNECTNODECONTAINER_CONNECTNODES:
					property.SetValue(connectivityNodes);
					break;

				default:
					base.GetProperty(property);
					break;
			}
		}

		public override void SetProperty(Property property)
		{
			switch (property.Id)
			{
				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

		#region IReference implementation

		public override bool IsReferenced
		{
			get
			{
				return (connectivityNodes.Count > 0) || base.IsReferenced;
			}
		}

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if (connectivityNodes != null && connectivityNodes.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.CONNECTNODECONTAINER_CONNECTNODES] = connectivityNodes.GetRange(0, connectivityNodes.Count);
			}

			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.CONNECTNODE_CONNECTNODECONTAINER:
					connectivityNodes.Add(globalId);
					break;

				default:
					base.AddReference(referenceId, globalId);
					break;
			}
		}

		public override void RemoveReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.CONNECTNODE_CONNECTNODECONTAINER:

					if (connectivityNodes.Contains(globalId))
					{
						connectivityNodes.Remove(globalId);
					}
					else
					{
						CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
					}

					break;
				default:
					base.RemoveReference(referenceId, globalId);
					break;
			}
		}

		#endregion IReference implementation
	}
}
