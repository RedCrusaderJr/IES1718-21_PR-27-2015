using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;


namespace FTN.Services.NetworkModelService.DataModel.Wires
{
	public class ACLineSegment : Conductor
	{
		private float b0ch;
		private float bch;
		private float g0ch;
		private float gch;
		private float r;
		private float r0;
		private float x;
		private float x0;

		public ACLineSegment(long globalId)
			: base(globalId)
		{
		}

		public float B0CH
		{
			get { return b0ch; }
			set { b0ch = value;}
		}

		public float BCH
		{
			get { return bch; }
			set { bch = value; }
		}

		public float G0CH
		{
			get { return g0ch; }
			set { g0ch = value; }
		}

		public float GCH
		{
			get { return gch; }
			set { gch = value; }
		}

		public float R
		{
			get { return r; }
			set { r = value; }
		}

		public float R0
		{
			get { return r0; }
			set { r0 = value; }
		}

		public float X
		{
			get { return x; }
			set { x = value; }
		}

		public float X0
		{
			get { return x0; }
			set { x0 = value; }
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				ACLineSegment acs = (ACLineSegment)obj;
				return ((acs.b0ch == this.b0ch) && (acs.bch == this.bch) &&
						(acs.g0ch == this.g0ch) && (acs.gch == this.gch) &&
						(acs.r == this.r) && (acs.r0 == this.r0) &&
						(acs.x == this.x) && (acs.x0 == this.x0));
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

		public override bool HasProperty(ModelCode t)
		{
			switch (t)
			{				
				case ModelCode.ACLINESEG_B0CH:
				case ModelCode.ACLINESEG_BCH:
				case ModelCode.ACLINESEG_G0CH:
				case ModelCode.ACLINESEG_GCH:
				case ModelCode.ACLINESEG_R:
				case ModelCode.ACLINESEG_R0:
				case ModelCode.ACLINESEG_X:
				case ModelCode.ACLINESEG_X0:
					return true;

				default:
					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.ACLINESEG_B0CH:
					prop.SetValue(b0ch);
					break;
				case ModelCode.ACLINESEG_BCH:
					prop.SetValue(bch);
					break;
				case ModelCode.ACLINESEG_G0CH:
					prop.SetValue(g0ch);
					break;
				case ModelCode.ACLINESEG_GCH:
					prop.SetValue(gch);
					break;
				case ModelCode.ACLINESEG_R:
					prop.SetValue(r);
					break;
				case ModelCode.ACLINESEG_R0:
					prop.SetValue(r0);
					break;
				case ModelCode.ACLINESEG_X:
					prop.SetValue(x);
					break;
				case ModelCode.ACLINESEG_X0:
					prop.SetValue(x0);
					break;

				default:
					base.GetProperty(prop);
					break;
			}
		}

		public override void SetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.ACLINESEG_B0CH:
					b0ch = property.AsFloat();
					break;
				case ModelCode.ACLINESEG_BCH:
					bch = property.AsFloat();
					break;
				case ModelCode.ACLINESEG_G0CH:
					g0ch = property.AsFloat();
					break;
				case ModelCode.ACLINESEG_GCH:
					gch = property.AsFloat();
					break;
				case ModelCode.ACLINESEG_R:
					r = property.AsFloat();
					break;
				case ModelCode.ACLINESEG_R0:
					r0 = property.AsFloat();
					break;
				case ModelCode.ACLINESEG_X:
					x = property.AsFloat();
					break;
				case ModelCode.ACLINESEG_X0:
					x0 = property.AsFloat();
					break;

				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

	}
}
