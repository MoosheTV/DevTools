using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Devtools.Client.Helpers;
using Devtools.Client.Menus;

namespace Devtools.Client.Controllers
{
	public class InteriorMenu : Menu
	{
		/// <summary>
		/// List of online interiors and interior props.
		/// Not all interiors are in here. For a list of interiors, check out: https://wiki.gt-mp.net/index.php?title=Online_Interiors_and_locations
		/// Not all interior props are in here. For a list of interior props, check out: https://wiki.gt-mp.net/index.php?title=InteriorPropList
		/// </summary>
		internal static readonly List<InteriorModel> Interiors = new List<InteriorModel> {
			// Online Bunker Exteriors
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Desert Bunker", "gr_case0_bunkerclosed", new Vector3(848.6175f, 2996.567f, 45.81612f)),
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Smoke Tree Bunker", "gr_case1_bunkerclosed", new Vector3(2126.785f, 3335.04f, 48.21422f)),
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Scrapyard Bunker", "gr_case2_bunkerclosed", new Vector3(2493.654f, 3140.399f, 51.28789f)),
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Oil Fields Bunker", "gr_case3_bunkerclosed", new Vector3(481.0465f, 2995.135f, 43.96672f)),
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Raton Canyon Bunker", "gr_case4_bunkerclosed", new Vector3(-391.3216f, 4363.728f, 58.65862f)),
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Grapeseed Bunker", "gr_case5_bunkerclosed", new Vector3(1823.961f, 4708.14f, 42.4991f)),
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Farmhouse Bunker", "gr_case6_bunkerclosed", new Vector3(1570.372f, 2254.549f, 78.89397f)),
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Paleto Bunker", "gr_case7_bunkerclosed", new Vector3(-783.0755f, 5934.686f, 24.31475f)),
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Route 68 Bunker", "gr_case9_bunkerclosed", new Vector3(24.43542f, 2959.705f, 58.35517f)),
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Zancudo Bunker", "gr_case10_bunkerclosed", new Vector3(-3058.714f, 3329.19f, 12.5844f)),
			new InteriorModel(InteriorCategory.OnlineBunkerExterior, "Route 1 Bunker", "gr_case11_bunkerclosed", new Vector3(-3180.466f, 1374.192f, 19.9597f)),

			// Online Apartments
			new InteriorModel(InteriorCategory.OnlineApartments, "Modern Apartment 1", "apa_v_mp_h_01_a", new Vector3(-786.8663f, 315.7642f, 217.6385f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Modern Apartment 2", "apa_v_mp_h_01_c", new Vector3(-786.9563f, 315.6229f, 187.9136f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Modern Apartment 3", "apa_v_mp_h_01_b", new Vector3(-774.0126f, 342.0428f, 196.6864f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Moody Apartment 1", "apa_v_mp_h_02_a", new Vector3(-787.0749f, 315.8198f, 217.6386f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Moody Apartment 2", "apa_v_mp_h_02_c", new Vector3(-786.8195f, 315.5634f, 187.9137f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Moody Apartment 3", "apa_v_mp_h_02_b", new Vector3(-774.1382f, 342.0316f, 196.6864f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Vibrant Apartment 1", "apa_v_mp_h_03_a", new Vector3(-786.6245f, 315.6175f, 217.6385f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Vibrant Apartment 2", "apa_v_mp_h_03_c", new Vector3(-786.9584f, 315.7974f, 187.9135f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Vibrant Apartment 3", "apa_v_mp_h_03_b", new Vector3(-774.0223f, 342.1718f, 196.6863f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Sharp Apartment 1", "apa_v_mp_h_04_a", new Vector3(-787.0902f, 315.7039f, 217.6384f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Sharp Apartment 2", "apa_v_mp_h_04_c", new Vector3(-787.0155f, 315.7071f, 187.9135f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Sharp Apartment 3", "apa_v_mp_h_04_b", new Vector3(-773.8976f, 342.1525f, 196.6863f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Monochrome Apartment 1", "apa_v_mp_h_05_a", new Vector3(-786.9887f, 315.7393f, 217.6386f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Monochrome Apartment 2", "apa_v_mp_h_05_c", new Vector3(-786.8809f, 315.6634f, 187.9136f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Monochrome Apartment 3", "apa_v_mp_h_05_b", new Vector3(-774.0675f, 342.0773f, 196.6864f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Seductive Apartment 1", "apa_v_mp_h_06_a", new Vector3(-787.1423f, 315.6943f, 217.6384f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Seductive Apartment 2", "apa_v_mp_h_06_c", new Vector3(-787.0961f, 315.815f, 187.9135f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Seductive Apartment 3", "apa_v_mp_h_06_b", new Vector3(-773.9552f, 341.9892f, 196.6862f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Regal Apartment 1", "apa_v_mp_h_07_a", new Vector3(-787.029f, 315.7113f, 217.6385f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Regal Apartment 2", "apa_v_mp_h_07_c", new Vector3(-787.0574f, 315.6567f, 187.9135f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Regal Apartment 3", "apa_v_mp_h_07_b", new Vector3(-774.0109f, 342.0965f, 196.6863f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Aqua Apartment 1", "apa_v_mp_h_08_a", new Vector3(-786.9469f, 315.5655f, 217.6383f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Aqua Apartment 2", "apa_v_mp_h_08_c", new Vector3(-786.9756f, 315.723f, 187.9134f)),
			new InteriorModel(InteriorCategory.OnlineApartments, "Aqua Apartment 3", "apa_v_mp_h_08_b", new Vector3(-774.0349f, 342.0296f, 196.6862f)),

			// Arcadius Business Center
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Executive Rich", "ex_dt1_02_office_02b", new Vector3(-141.1987f, -620.913f, 168.8205f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Executive Cool", "ex_dt1_02_office_02c", new Vector3(-141.5429f, -620.9524f, 168.8204f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Executive Contrast", "ex_dt1_02_office_02a", new Vector3(-141.2896f, -620.9618f, 168.8204f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Old Spice Warm", "ex_dt1_02_office_01a", new Vector3(-141.4966f, -620.8292f, 168.8204f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Old Spice Classical", "ex_dt1_02_office_01b", new Vector3(-141.3997f, -620.9006f, 168.8204f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Old Spide Vintage", "ex_dt1_02_office_01c", new Vector3(-141.5361f, -620.9186f, 168.8204f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Power Broker Ice", "ex_dt1_02_office_03a", new Vector3(-141.392f, -621.0451f, 168.8204f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Power Broker Conservative", "ex_dt1_02_office_03b", new Vector3(-141.1945f, -620.8729f, 168.8204f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Power Broker Polished", "ex_dt1_02_office_03c", new Vector3(-141.4924f, -621.0035f, 168.8205f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Garage 1", "imp_dt1_02_cargarage_a", new Vector3(-191.0133f, -579.1428f, 135.0000f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Garage 2", "imp_dt1_02_cargarage_b", new Vector3(-117.4989f, -568.1132f, 135.0000f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Garage 3", "imp_dt1_02_cargarage_c", new Vector3(-136.0780f, -630.1852f, 135.0000f)),
			new InteriorModel(InteriorCategory.ArcadiusOffices, "Mod Shop", "imp_dt1_02_modgarage", new Vector3(-146.6166f, -596.6301f, 166.0000f)),

			// Maze Bank Building
			new InteriorModel(InteriorCategory.MazeOffices, "Executive Rich", "ex_dt_11_office_02b", new Vector3(-75.8466f, -826.9893f, 243.3859f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Executive Cool", "ex_dt_11_office_02c", new Vector3(-75.49945f, -827.05f, 243.386f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Executive Contrast", "ex_dt_11_office_02a", new Vector3(-75.49827f, -827.1889f, 243.386f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Old Spice Warm", "ex_dt_11_office_01a", new Vector3(-75.44054f, -827.1487f, 243.3859f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Old Spice Classical", "ex_dt_11_office_01b", new Vector3(-75.63942f, -827.1022f, 243.3859f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Old Spice Vintage", "ex_dt_11_office_01c", new Vector3(-75.47446f, -827.2621f, 243.386f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Power Broker Ice", "ex_dt_11_office_03a", new Vector3(-75.56978f, -827.1152f, 243.3859f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Power Broker Conservative", "ex_dt_11_office_03b", new Vector3(-75.51953f, -827.0786f, 243.3859f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Power Broker Polished", "ex_dt_11_office_03c", new Vector3(-75.41915f, -827.1118f, 243.3858f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Garage 1", "imp_dt1_11_cargarage_a", new Vector3(-84.2193f, -823.0851f, 221.0000f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Garage 2", "imp_dt1_11_cargarage_b", new Vector3(-69.8627f, -824.7498f, 221.0000f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Garage 3", "imp_dt1_11_cargarage_c", new Vector3(-80.4318f, -813.2536f, 221.0000f)),
			new InteriorModel(InteriorCategory.MazeOffices, "Mod Shop", "imp_dt1_11_modgarage", new Vector3(-73.9039f, -821.6204f, 284.0000f)),

			// Lom Bank Offices
			new InteriorModel(InteriorCategory.LomBankOffices, "Executive Rich", "ex_sm_13_office_02b", new Vector3(-1579.756f, -565.0661f, 108.523f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Executive Cool", "ex_sm_13_office_02c", new Vector3(-1579.678f, -565.0034f, 108.5229f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Executive Contrast", "ex_sm_13_office_02a", new Vector3(-1579.583f, -565.0399f, 108.5229f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Old Spice Warm", "ex_sm_13_office_01a", new Vector3(-1579.702f, -565.0366f, 108.5229f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Old Spice Classical", "ex_sm_13_office_01b", new Vector3(-1579.643f, -564.9685f, 108.5229f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Old Spice Vintage", "ex_sm_13_office_01c", new Vector3(-1579.681f, -565.0003f, 108.523f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Power Broker Ice", "ex_sm_13_office_03a", new Vector3(-1579.677f, -565.0689f, 108.5229f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Power Broker Conservative", "ex_sm_13_office_03b", new Vector3(-1579.708f, -564.9634f, 108.5229f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Power Broker Polished", "ex_sm_13_office_03b", new Vector3(-1579.693f, -564.8981f, 108.5229f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Garage 1", "imp_sm_13_cargarage_a", new Vector3(-1581.1120f, -567.2450f, 85.5000f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Garage 2", "imp_sm_13_cargarage_b", new Vector3(-1568.7390f, -562.0455f, 85.5000f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Garage 3", "imp_sm_13_cargarage_c", new Vector3(-1563.5570f, -574.4314f, 85.5000f)),
			new InteriorModel(InteriorCategory.LomBankOffices, "Mod Shop", "imp_sm_13_modgarage", new Vector3(-1578.0230f, -576.4251f, 104.2000f)),

			// Del Perro Offices
			new InteriorModel(InteriorCategory.DelPerroOffices, "Executive Rich", "ex_sm_15_office_02b", new Vector3(-1392.667f, -480.4736f, 72.04217f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Executive Cool", "ex_sm_15_office_02c", new Vector3(-1392.542f, -480.4011f, 72.04211f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Executive Contrast", "ex_sm_15_office_02a", new Vector3(-1392.626f, -480.4856f, 72.04212f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Old Spice Warm", "ex_sm_15_office_01a", new Vector3(-1392.617f, -480.6363f, 72.04208f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Old Spice Classical", "ex_sm_15_office_01b", new Vector3(-1392.532f, -480.7649f, 72.04207f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Old Spice Vintage", "ex_sm_15_office_01c", new Vector3(-1392.611f, -480.5562f, 72.04214f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Power Broker Ice", "ex_sm_15_office_03a", new Vector3(-1392.563f, -480.549f, 72.0421f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Power Broker Conservative", "ex_sm_15_office_03b", new Vector3(-1392.528f, -480.475f, 72.04206f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Power Broker Polished", "ex_sm_15_office_03c", new Vector3(-1392.416f, -480.7485f, 72.04207f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Garage 1", "imp_sm_15_cargarage_a", new Vector3(-1388.8400f, -478.7402f, 56.1000f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Garage 2", "imp_sm_15_cargarage_b", new Vector3(-1388.8600f, -478.7574f, 48.1000f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Garage 3", "imp_sm_15_cargarage_c", new Vector3(-1374.6820f, -474.3586f, 56.1000f)),
			new InteriorModel(InteriorCategory.DelPerroOffices, "Mod Shop", "imp_sm_15_modgarage", new Vector3(-1391.2450f, -473.9638f, 77.2000f)),

			// Clubhouses
			new InteriorModel(InteriorCategory.BikerClubs, "Clubhouse 1", "bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo", new Vector3(1107.04f, -3157.399f, -37.51859f)) {
				InteriorProps = new Dictionary<string, bool> {
					{"decorative_1", false},
					{"decorative_2", false},
					{"furnishings_01", false},
					{"furnishings_02", false},
					{"walls_01", false},
					{"walls_02", false},
					{"mural_01", false},
					{"mural_02", false},
					{"mural_03", false},
					{"mural_04", false},
					{"mural_05", false},
					{"mural_06", false},
					{"mural_07", false},
					{"mural_08", false},
					{"mural_09", false},
					{"cash_stash1", false},
					{"cash_stash2", false},
					{"cash_stash3", false},
					{"coke_stash1", false},
					{"coke_stash2", false},
					{"coke_stash3", false},
					{"counterfeit_stash1", false},
					{"counterfeit_stash2", false},
					{"counterfeit_stash3", false},
					{"weed_stash1", false},
					{"weed_stash2", false},
					{"weed_stash3", false},
					{"id_stash1", false},
					{"id_stash2", false},
					{"id_stash3", false},
					{"meth_stash1", false},
					{"meth_stash2", false},
					{"meth_stash3", false},
					{"gun_locker", false},
					{"mod_booth", false},
					{"no_gun_locker", false},
					{"no_mod_booth", false},
				}
			},
			new InteriorModel(InteriorCategory.BikerClubs, "Clubhouse 2", "bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo", new Vector3(998.4809f, -3164.711f, -38.90733f)) {
				InteriorProps = new Dictionary<string, bool> {
					{"decorative_1", false},
					{"decorative_2", false},
					{"furnishings_01", false},
					{"furnishings_02", false},
					{"walls_01", false},
					{"walls_02", false},
					{"lower_walls_default", false},
					{"cash_large", false},
					{"cash_medium", false},
					{"cash_small", false},
					{"coke_large", false},
					{"coke_medium", false},
					{"coke_small", false},
					{"counterfeit_large", false},
					{"counterfeit_medium", false},
					{"counterfeit_small", false},
					{"id_large", false},
					{"id_medium", false},
					{"id_small", false},
					{"meth_large", false},
					{"meth_medium", false},
					{"meth_small", false},
					{"weed_large", false},
					{"weed_medium", false},
					{"weed_small", false},
					{"gun_locker", false},
					{"mod_booth", false},
					{"no_gun_locker", false},
					{"no_mod_booth", false}
				}
			},
			new InteriorModel(InteriorCategory.BikerClubs, "Meth Lab", "bkr_biker_interior_placement_interior_2_biker_dlc_int_ware01_milo", new Vector3(1009.5f, -3196.6f, -38.99682f)) {
				InteriorProps = new Dictionary<string, bool> {
					{"meth_lab_basic", false},
					{"meth_lab_empty", false},
					{"meth_lab_production", false},
					{"meth_lab_security_high", false},
					{"meth_lab_setup", false},
					{"meth_lab_upgrade", false}
				}
			},
			new InteriorModel(InteriorCategory.BikerClubs, "Weed Farm", "bkr_biker_interior_placement_interior_3_biker_dlc_int_ware02_milo", new Vector3(1051.491f, -3196.536f, -39.14842f)) {
				InteriorProps = new Dictionary<string, bool> {
					{"light_growtha_stage23_standard", false},
					{"light_growtha_stage23_upgrade", false},
					{"light_growthb_stage23_standard", false},
					{"light_growthb_stage23_upgrade", false},
					{"light_growthc_stage23_standard", false},
					{"light_growthc_stage23_upgrade", false},
					{"light_growthd_stage23_standard", false},
					{"light_growthd_stage23_upgrade", false},
					{"light_growthe_stage23_standard", false},
					{"light_growthe_stage23_upgrade", false},
					{"light_growthf_stage23_standard", false},
					{"light_growthf_stage23_upgrade", false},
					{"light_growthg_stage23_standard", false},
					{"light_growthg_stage23_upgrade", false},
					{"light_growthh_stage23_standard", false},
					{"light_growthh_stage23_upgrade", false},
					{"light_growthi_stage23_standard", false},
					{"light_growthi_stage23_upgrade", false},
					{"weed_growtha_stage1",false},
					{"weed_growtha_stage2",false},
					{"weed_growtha_stage3",false},
					{"weed_growthb_stage1",false},
					{"weed_growthb_stage2",false},
					{"weed_growthb_stage3",false},
					{"weed_growthc_stage1",false},
					{"weed_growthc_stage2",false},
					{"weed_growthc_stage3",false},
					{"weed_growthd_stage1",false},
					{"weed_growthd_stage2",false},
					{"weed_growthd_stage3",false},
					{"weed_growthe_stage1",false},
					{"weed_growthe_stage2",false},
					{"weed_growthe_stage3",false},
					{"weed_growthf_stage1",false},
					{"weed_growthf_stage2",false},
					{"weed_growthf_stage3",false},
					{"weed_growthg_stage1",false},
					{"weed_growthg_stage2",false},
					{"weed_growthg_stage3",false},
					{"weed_growthh_stage1",false},
					{"weed_growthh_stage2",false},
					{"weed_growthh_stage3",false},
					{"weed_growthi_stage1",false},
					{"weed_growthi_stage2",false},
					{"weed_growthi_stage3",false},
					{"weed_hosea",false},
					{"weed_hoseb",false},
					{"weed_hosec",false},
					{"weed_hosed",false},
					{"weed_hosee",false},
					{"weed_hosef",false},
					{"weed_hoseg",false},
					{"weed_hoseh",false},
					{"weed_hosei",false},
					{"weed_chairs",false},
					{"weed_drying",false},
					{"weed_low_security",false},
					{"weed_production",false},
					{"weed_security_upgrade",false},
					{"weed_set_up",false},
					{"weed_standard_equip",false},
					{"weed_upgrade_equip",false}
				}
			},
			new InteriorModel(InteriorCategory.BikerClubs, "Cocaine Kitchen", "bkr_biker_interior_placement_interior_4_biker_dlc_int_ware03_milo", new Vector3(1093.6f, -3196.6f, -38.99841f)),
			new InteriorModel(InteriorCategory.BikerClubs, "Money Printing", "bkr_biker_interior_placement_interior_5_biker_dlc_int_ware04_milo", new Vector3(1121.897f, -3195.338f, -40.4025f)),
			new InteriorModel(InteriorCategory.BikerClubs, "Passport Forgery", "bkr_biker_interior_placement_interior_6_biker_dlc_int_ware05_milo", new Vector3(1165f, -3196.6f, -39.01306f)),

			// Warehouses
			new InteriorModel(InteriorCategory.Warehouses, "Small Warehouse", "ex_exec_warehouse_placement_interior_1_int_warehouse_s_dlc_milo_", new Vector3(1094.988f, -3101.776f, -39.00363f) ),
			new InteriorModel(InteriorCategory.Warehouses, "Medium Warehouse", "ex_exec_warehouse_placement_interior_0_int_warehouse_m_dlc_milo_", new Vector3(1056.486f, -3105.724f, -39.00439f)),
			new InteriorModel(InteriorCategory.Warehouses, "Large Warehouse", "ex_exec_warehouse_placement_interior_2_int_warehouse_l_dlc_milo_", new Vector3(1006.967f, -3102.079f, -39.0035f)),
			new InteriorModel(InteriorCategory.Warehouses, "Vehicle Warehouse", "imp_impexp_interior_placement_interior_1_impexp_intwaremed_milo_", new Vector3(994.5925f, -3002.594f, -39.64699f)),
		};

		/// <summary>
		/// Restricts multiple interiors being loaded with the same category
		/// </summary>
		internal static readonly List<InteriorCategory> RestrictMultiple = new List<InteriorCategory> {
			InteriorCategory.Apartments,
			InteriorCategory.ArcadiusOffices,
			InteriorCategory.LomBankOffices,
			InteriorCategory.MazeOffices,
			InteriorCategory.DelPerroOffices,
			InteriorCategory.BikerClubs,
			InteriorCategory.Warehouses,
			InteriorCategory.OnlineApartments
		};

		public InteriorMenu( Client client, Menu parent ) : base( "Interior Menu", parent ) {
			var presets = new Menu( "Preset Interiors", this );
			var categories = new Dictionary<InteriorCategory, Menu>();
			foreach( var interior in Interiors ) {
				Menu category;
				if( !categories.ContainsKey( interior.Category ) ) {
					category = new Menu( Enum.GetName( typeof( InteriorCategory ), interior.Category )?.ToTitleCase() ?? "", presets );
					categories.Add( interior.Category, category );
				}
				else {
					category = categories[interior.Category];
				}
				category.Add( new MenuItemSubMenu( client, category, new MenuItemInterior( client, category, interior ), interior.Name ) );
			}
			foreach( var category in categories ) {
				presets.Add( new MenuItemSubMenu( client, presets, category.Value, Enum.GetName( typeof( InteriorCategory ), category.Key )?.ToTitleCase() ?? "" ) );
			}
			Add( new MenuItemSubMenu( client, this, presets, "Preset Interiors" ) );

			var custom = new MenuItem( client, this, "Toggle Custom Interior" );
			custom.Activate += async () => {
				var input = await UiHelper.PromptTextInput( controller: client.Menu );
				if( !string.IsNullOrWhiteSpace( input ) ) {
					var isActive = Function.Call<bool>( Hash.IS_IPL_ACTIVE, input );
					UiHelper.ShowNotification( $"Attempting to {(isActive ? "~r~unload" : "~g~load")} ~y~{input}~s~." );
					Function.Call( !isActive ? Hash.REQUEST_IPL : Hash.REMOVE_IPL, input );
				}
			};
			Add( custom );

			var customProps = new MenuItem( client, this, "Toggle Custom Interior Props" );
			customProps.Activate += async () => {
				var input = await UiHelper.PromptTextInput( controller: client.Menu );
				if( !string.IsNullOrWhiteSpace( input ) ) {
					var id = Function.Call<int>( Hash.GET_INTERIOR_AT_COORDS, Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y, Game.PlayerPed.Position.Z );
					var isActive = Function.Call<bool>( Hash._IS_INTERIOR_PROP_ENABLED, id );
					UiHelper.ShowNotification( $"Attempting to {(isActive ? "~r~unload" : "~g~load")} ~y~{input}~s~." );
					Function.Call( !isActive ? Hash._ENABLE_INTERIOR_PROP : Hash._DISABLE_INTERIOR_PROP, id, input );
					Function.Call( Hash.REFRESH_INTERIOR, id );
				}
			};
			Add( customProps );
		}
	}

	public enum InteriorCategory
	{
		OnlineBunkerExterior,
		OnlineApartments,
		ArcadiusOffices,
		MazeOffices,
		LomBankOffices,
		DelPerroOffices,
		BikerClubs,
		Warehouses,
		SpecialLocations,
		Apartments,
		Miscellaneous,
		AfterHours,
		ConveienceStores,
		Garages
	}

	public class InteriorModel
	{
		public InteriorCategory Category { get; set; }
		public string Name { get; set; }
		public List<string> Interior { get; set; }
		public Vector3 Position { get; set; }
		public bool IsLoaded { get; set; }
		public Dictionary<string, bool> InteriorProps { get; set; } = new Dictionary<string, bool>();

		public InteriorModel( InteriorCategory category, string name, string interior, Vector3 position ) {
			Category = category;
			Name = name;
			Interior = new List<string> { interior };
			Position = position;
		}
	}

	public class MenuItemInterior : Menu
	{
		public InteriorModel Interior { get; set; }

		public MenuItemInterior( Client client, Menu owner, InteriorModel interior ) : base( interior.Name, owner ) {
			Interior = interior;

			var tele = new MenuItem( client, owner, "Teleport" );
			tele.Activate += async () => {
				API.DoScreenFadeOut( 200 );
				await BaseScript.Delay( 250 );
				Game.PlayerPed.Position = interior.Position;
				Game.PlayerPed.IsPositionFrozen = true;
				await BaseScript.Delay( 50 );
				Game.PlayerPed.IsPositionFrozen = false;
				API.DoScreenFadeIn( 200 );
			};
			Add( tele );

			foreach( var inter in interior.Interior ) {
				var label = inter;
				if( label.Length > 16 ) {
					label = inter.Substring( 0, 16 ) + "...";
				}
				var toggle = new MenuItemCheckbox( client, owner, label, interior.IsLoaded ) {
					IsChecked = () => interior.IsLoaded
				};
				toggle.Activate += () => {
					foreach( var i in InteriorMenu.Interiors ) {
						if( !InteriorMenu.RestrictMultiple.Contains( i.Category ) || i.Category != interior.Category || !i.IsLoaded || i.Interior.All( p => p != inter ) ) continue;
						Function.Call( Hash.REMOVE_IPL, inter );
						i.IsLoaded = false;
					}

					interior.IsLoaded = !interior.IsLoaded;
					Function.Call( interior.IsLoaded ? Hash.REQUEST_IPL : Hash.REMOVE_IPL, inter );
					return Task.FromResult( 0 );
				};
				Add( toggle );
			}

			if( interior.InteriorProps.Any() ) {
				var propMenu = new Menu( "Interior Props", this );

				foreach( var kvp in interior.InteriorProps ) {
					var label = kvp.Key;
					if( label.Length > 16 ) {
						label = kvp.Key.Substring( 0, 16 ) + "...";
					}
					var prop = new MenuItemCheckbox( client, propMenu, label, kvp.Value ) {
						IsChecked = () => interior.InteriorProps[kvp.Key]
					};
					prop.Activate += () => {
						var id = Function.Call<int>( Hash.GET_INTERIOR_AT_COORDS, interior.Position.X, interior.Position.Y, interior.Position.Z );
						interior.InteriorProps[kvp.Key] = !interior.InteriorProps[kvp.Key];
						Function.Call( interior.InteriorProps[kvp.Key] ? Hash._ENABLE_INTERIOR_PROP : Hash._DISABLE_INTERIOR_PROP, id, kvp.Key );
						Function.Call( Hash.REFRESH_INTERIOR, id );
						return Task.FromResult( 0 );
					};
					propMenu.Add( prop );
				}
				Add( new MenuItemSubMenu( client, this, propMenu, "Interior Props" ) );
			}
		}
	}
}
