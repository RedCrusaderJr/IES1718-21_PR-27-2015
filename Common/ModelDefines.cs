using System;
using System.Collections.Generic;
using System.Text;

namespace FTN.Common
{
	
	public enum DMSType : short
	{		
		MASK_TYPE							= unchecked((short)0xFFFF),

        CONNECTNODE                         = 0x0001,
        CONNECTNODECONTAINER                = 0x0002,
        TERMINAL                            = 0x0003,
        SERIESCOMPENSATOR                   = 0x0004,
        ACLINESEG                           = 0x0005,
        DCLINESEG                           = 0x0006,
	}

    [Flags]
	public enum ModelCode : long
	{
		IDOBJ								= 0x1000000000000000,
		IDOBJ_GID							= 0x1000000000000104,
		IDOBJ_ALIASNAME 					= 0x1000000000000207,
		IDOBJ_MRID							= 0x1000000000000307,
		IDOBJ_NAME							= 0x1000000000000407,	

        PSR                                 = 0x1100000000000000,

        CONNECTNODE                         = 0x1200000000010000,
        CONNECTNODE_DESCRIPTION             = 0x1200000000010107,
        CONNECTNODE_CONNECTNODECONTAINER    = 0x1200000000010209,
        CONNECTNODE_TERMINALS               = 0x1200000000010319,

        TERMINAL                            = 0x1300000000030000,
        TERMINAL_CONDEQUIP                  = 0x1300000000030109,
        TERMINAL_CONNECTNODE                = 0x1300000000030209,

        CONNECTNODECONTAINER                = 0x1110000000020000,
        CONNECTNODECONTAINER_CONNECTNODES   = 0x1110000000020119,

        EQUIPMENT                           = 0x1120000000000000,

        CONDEQUIP                           = 0x1121000000000000,
        CONDEQUIP_TERMINALS                 = 0x1121000000000119,

        CONDUCTOR                           = 0x1121100000000000,
        CONDUCTOR_LENGTH                    = 0x1121100000000105,

        SERIESCOMPENSATOR                   = 0x1121200000040000,
        SERIESCOMPENSATOR_R                 = 0x1121200000040105,
        SERIESCOMPENSATOR_R0                = 0x1121200000040205,
        SERIESCOMPENSATOR_X                 = 0x1121200000040305,
        SERIESCOMPENSATOR_X0                = 0x1121200000040405,

        ACLINESEG                           = 0x1121110000050000,
        ACLINESEG_B0CH                      = 0x1121110000050105,
        ACLINESEG_BCH                       = 0x1121110000050205,
        ACLINESEG_G0CH                      = 0x1121110000050305,
        ACLINESEG_GCH                       = 0x1121110000050405,
        ACLINESEG_R                         = 0x1121110000050505,
        ACLINESEG_R0                        = 0x1121110000050605,
        ACLINESEG_X                         = 0x1121110000050705,
        ACLINESEG_X0                        = 0x1121110000050805,

        DCLINESEG                           = 0x1121120000060000,
    }

    [Flags]
	public enum ModelCodeMask : long
	{
		MASK_TYPE			 = 0x00000000ffff0000,
		MASK_ATTRIBUTE_INDEX = 0x000000000000ff00,
		MASK_ATTRIBUTE_TYPE	 = 0x00000000000000ff,

		MASK_INHERITANCE_ONLY = unchecked((long)0xffffffff00000000),
		MASK_FIRSTNBL		  = unchecked((long)0xf000000000000000),
		MASK_DELFROMNBL8	  = unchecked((long)0xfffffff000000000),		
	}																		
}


