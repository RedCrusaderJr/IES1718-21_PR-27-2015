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
	public class SeriesCompensator : ConductingEquipment
	{
		private float r;
		private float r0;
		private float x;
		private float x0;

		public SeriesCompensator(long globalId)
			: base(globalId)
		{
		}

		public float R
		{
			get { return r; }
			set { r = value;}
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
				SeriesCompensator sc = (SeriesCompensator)obj;
				return (sc.r == this.r && sc.r0 == this.r0 && sc.x == this.x && sc.x0 == this.x0);
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
				case ModelCode.SERIESCOMPENSATOR_R:
				case ModelCode.SERIESCOMPENSATOR_R0:
				case ModelCode.SERIESCOMPENSATOR_X:
				case ModelCode.SERIESCOMPENSATOR_X0:
					return true;

				default:
					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{

				case ModelCode.SERIESCOMPENSATOR_R:
					prop.SetValue(r);
					break;				

				case ModelCode.SERIESCOMPENSATOR_R0:
					prop.SetValue(r0);
					break;

				case ModelCode.SERIESCOMPENSATOR_X:
					prop.SetValue(x);
					break;

				case ModelCode.SERIESCOMPENSATOR_X0:
					prop.SetValue(x0);
					break;

				default:
					base.GetProperty(prop);
					break;
			}
		}

		public override void SetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.SERIESCOMPENSATOR_R:
					r = prop.AsFloat();
					break;				

				case ModelCode.SERIESCOMPENSATOR_R0:
					r0 = prop.AsFloat();
					break;

				case ModelCode.SERIESCOMPENSATOR_X:
					x = prop.AsFloat();
					break;

				case ModelCode.SERIESCOMPENSATOR_X0:
					x0 = prop.AsFloat();
					break;
			
				default:
					base.SetProperty(prop);
					break;
			}
		}

		#endregion IAccess implementation

	}
}
