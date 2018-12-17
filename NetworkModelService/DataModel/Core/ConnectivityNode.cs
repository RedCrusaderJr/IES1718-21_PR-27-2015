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
	public class ConnectivityNode : IdentifiedObject
	{
		private string description = string.Empty;
		private long connectivityNodeContainer;
		private List<long> terminals = new List<long>();
		public ConnectivityNode(long globalId)
			: base(globalId)
		{
		}

		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		public long ConnectivityNodeContainer
		{
			get { return connectivityNodeContainer; }
			set { connectivityNodeContainer = value; }
		}

		public List<long> Terminals
		{
			get { return terminals; }
			set { terminals = value; }
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				ConnectivityNode cn = (ConnectivityNode)obj;
				return ((cn.description == this.description) && (cn.connectivityNodeContainer == this.connectivityNodeContainer)
															 && (CompareHelper.CompareLists(cn.terminals, this.terminals, true)));
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
				case ModelCode.CONNECTNODE_DESCRIPTION:
				case ModelCode.CONNECTNODE_CONNECTNODECONTAINER:
				case ModelCode.CONNECTNODE_TERMINALS:
					return true;

				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.CONNECTNODE_DESCRIPTION:
					property.SetValue(description);
					break;
				case ModelCode.CONNECTNODE_CONNECTNODECONTAINER:
					property.SetValue(connectivityNodeContainer);
					break;
				case ModelCode.CONNECTNODE_TERMINALS:
					property.SetValue(terminals);
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
				case ModelCode.CONNECTNODE_DESCRIPTION:
					description = property.AsString();
					break;
				case ModelCode.CONNECTNODE_CONNECTNODECONTAINER:
					connectivityNodeContainer = property.AsLong();
					break;

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
                return (terminals.Count > 0) || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (terminals != null && terminals.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.CONNECTNODE_TERMINALS] = terminals.GetRange(0, terminals.Count);
            }

            if (connectivityNodeContainer != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.CONNECTNODE_CONNECTNODECONTAINER] = new List<long>()
                {
                    connectivityNodeContainer,
                };
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.TERMINAL_CONNECTNODE:
                    terminals.Add(globalId);
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
                case ModelCode.TERMINAL_CONNECTNODE:

                    if (terminals.Contains(globalId))
                    {
                        terminals.Remove(globalId);
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
