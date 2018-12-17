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
	public class Terminal : IdentifiedObject
	{
		private long conductingEquipment;
		private long connectivityNode;
		public Terminal(long globalId)
			: base(globalId) 
		{
		}
	
		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				Terminal t = (Terminal)obj;
				return ((t.connectivityNode == this.connectivityNode) && (t.conductingEquipment == this.conductingEquipment));
			}
			else
			{
				return false;
			}
		}

		public long ConductingEquipment
		{
			get { return conductingEquipment; }
			set { conductingEquipment = value; }
		}

		public long ConnectivityNode
		{
			get { return connectivityNode; }
			set { connectivityNode = value; }
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
				case ModelCode.TERMINAL_CONDEQUIP:
				case ModelCode.TERMINAL_CONNECTNODE:
					return true;

				default:
					return base.HasProperty(property);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.TERMINAL_CONDEQUIP:
					property.SetValue(conductingEquipment);
					break;

				case ModelCode.TERMINAL_CONNECTNODE:
					property.SetValue(connectivityNode);
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
				case ModelCode.TERMINAL_CONDEQUIP:
					conductingEquipment = property.AsLong();
					break;

				case ModelCode.TERMINAL_CONNECTNODE:
					connectivityNode = property.AsLong();
					break;

				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

		#region IReference implementation
 
		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
		{
			if(conductingEquipment != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.TERMINAL_CONDEQUIP] = new List<long>()
				{
					conductingEquipment,
				};
			}

			if (connectivityNode != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.TERMINAL_CONNECTNODE] = new List<long>()
				{
					connectivityNode,
				};
			}

			base.GetReferences(references, refType);
		}
		
		#endregion IReference implementation
	}
}
