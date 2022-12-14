#region Usings
using System.Collections.Generic;
using System.Collections.Specialized;
using UttrekkFamilia.Modulus;
#endregion

namespace UttrekkFamilia
{
    internal class Mappings
    {
        #region Members
        private readonly bool UseSokrates = true;

        private readonly NameValueCollection SSBHovedkategori = new() { 
            { "01", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "02", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "03", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "04", "TILSYN_OG_KONTROLL" }, 
            { "05", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "06", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "07", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "08", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "09", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "10", "UNDERSØKELSE_OG_BEHANDLING_FRA_ANDRE_TJENESTER" }, 
            { "11", "UNDERSØKELSE_OG_BEHANDLING_FRA_ANDRE_TJENESTER" }, 
            { "12", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "13", "BOLIG" }, 
            { "14", "FOSTERHJEM" }, 
            { "15", "FOSTERHJEM" }, 
            { "16", "FOSTERHJEM" }, 
            { "17", "FOSTERHJEM" }, 
            { "18", "FOSTERHJEM" }, 
            { "19", "INSTITUSJON" }, 
            { "21", "INSTITUSJON" }, 
            { "23", "UNDERSØKELSE_OG_BEHANDLING_FRA_ANDRE_TJENESTER" }, 
            { "24", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "25", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "26", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "27", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "28", "NETTVERKSARBEID/SAMARBEID_MED_ANDRE_TJENESTER" }, 
            { "29", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "30", "BOLIG" }, 
            { "100", "INSTITUSJON" }, 
            { "101", "INSTITUSJON" }, 
            { "102", "INSTITUSJON" }, 
            { "103", "FOSTERHJEM" }, 
            { "104", "FOSTERHJEM" }, 
            { "105", "FOSTERHJEM" }, 
            { "106", "FOSTERHJEM" }, 
            { "107", "FOSTERHJEM" }, 
            { "108", "FOSTERHJEM" }, 
            { "109", "FOSTERHJEM" }, 
            { "110", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "111", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "112", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "113", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "114", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "115", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "116", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "117", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "118", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "119", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "120", "TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "122", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "123", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "124", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "125", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "126", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "127", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "128", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "129", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "130", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "131", "TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "132", "TILSYN_OG_KONTROLL" }, 
            { "133", "TILSYN_OG_KONTROLL" }, 
            { "134", "TILSYN_OG_KONTROLL" }, 
            { "135", "TILSYN_OG_KONTROLL" }, 
            { "136", "TILSYN_OG_KONTROLL" }, 
            { "137", "NETTVERKSARBEID/SAMARBEID_MED_ANDRE_TJENESTER" }, 
            { "138", "NETTVERKSARBEID/SAMARBEID_MED_ANDRE_TJENESTER" }, 
            { "139", "NETTVERKSARBEID/SAMARBEID_MED_ANDRE_TJENESTER" }, 
            { "140", "NETTVERKSARBEID/SAMARBEID_MED_ANDRE_TJENESTER" }, 
            { "141", "NETTVERKSARBEID/SAMARBEID_MED_ANDRE_TJENESTER" }, 
            { "142", "UNDERSØKELSE_OG_BEHANDLING_FRA_ANDRE_TJENESTER" }, 
            { "143", "UNDERSØKELSE_OG_BEHANDLING_FRA_ANDRE_TJENESTER" }, 
            { "144", "UNDERSØKELSE_OG_BEHANDLING_FRA_ANDRE_TJENESTER" },
            { "145", "UNDERSØKELSE_OG_BEHANDLING_FRA_ANDRE_TJENESTER" }, 
            { "146", "BOLIG" }, 
            { "147", "BOLIG" }, 
            { "148", "BOLIG" }, 
            { "149", "BOLIG" }
        };

        private readonly NameValueCollection SSBUnderkategori = new() { 
            { "01", "ØKONOMISK_HJELP_FOR_ØVRIG" }, 
            { "02", "BARNEHAGE" }, 
            { "03", "STØTTEKONTAKT" }, 
            { "04", "FRIVILLIG_TILSYN_I_HJEMMET" }, 
            { "05", "BESØKSHJEM/AVLASTNINGSTILTAK" }, 
            { "06", "HJEMMEKONSULENT/MILJØARBEIDER" }, 
            { "07", "SFO/AKTIVITETSSKOLE" }, 
            { "08", "FRITIDSAKTIVITETER" }, 
            { "09", "UTDANNING_OG_ARBEID" }, 
            { "10", "MEDISINSK_UNDERSØKELSE_OG_BEHANDLING_(BARNEVERNLOVEN_§_4-10)" }, 
            { "11", "BEHANDLING_AV_BARN_MED_SÆRLIGE_OPPLÆRINGSBEHOV_(BARNEVERNSLOVEN_§_4-11)" }, 
            { "12", "SENTRE_FOR_FORELDRE_OG_BARN" }, 
            { "13", "BOLIG_MED_OPPFØLGING_(INKLUDERER_OGSÅ_BOFELLESSKAP)" }, 
            { "14", "BEREDSKAPSHJEM_UTENOM_FAMILIE_OG_NÆRE_NETTVERK" }, 
            { "15", "FOSTERHJEM_UTENFOR_FAMILIE_OG_NÆRE_NETTVERK" }, 
            { "16", "FOSTERHJEM_I_FAMILIE_OG_NÆRE_NETTVERK" }, 
            { "17", "FOSTERHJEM_UTENFOR_FAMILIE_OG_NÆRE_NETTVERK" }, 
            { "18", "FOSTERHJEM_I_FAMILIE_OG_NÆRE_NETTVERK" }, 
            { "19", "BARNVERNSINSTITUSJONER_(GJELDER_ALLE_TYPER_BARNEVERNSINSTITUSJONER)" }, 
            { "21", "PLASSERING_I_INSTITUSJON_ETTER_ANNEN_LOV" }, 
            { "23", "PSYKISK_HELSEHJELP_FOR_BARN_OG_UNGE" }, 
            { "24", "MST_(MULTISYSTEMISK_TERAPI)" }, 
            { "25", "PMTO_(PARENT_MANAGEMENT_TRAINING_OREGON)" }, 
            { "26", "ANDRE_TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "27", "VEDTAK_OM_RÅD_OG_VEILEDNING" }, 
            { "28", "DELTAKELSE_I_ANSVARSGRUPPE/SAMARBEIDSTEAM" }, 
            { "29", "ANDRE_HJEMMEBASERTE_TILTAK" }, 
            { "30", "BOLIG_MED_OPPFØLGING_(INKLUDERER_OGSÅ_BOFELLESSKAP)" }, 
            { "100", "BARNVERNSINSTITUSJONER_(GJELDER_ALLE_TYPER_BARNEVERNSINSTITUSJONER)" }, 
            { "101", "PLASSERING_I_INSTITUSJON_ETTER_ANNEN_LOV" }, 
            { "102", "ANDRE_INSTITUSJONSTILTAK" }, 
            { "103", "FOSTERHJEM_I_FAMILIE_OG_NÆRE_NETTVERK" }, 
            { "104", "FOSTERHJEM_UTENFOR_FAMILIE_OG_NÆRE_NETTVERK" }, 
            { "105", "STATLIGE_FAMILIEHJEM_(GJELDER_FOSTERHJEM_SOM_STATEN_HAR_ANSVAR_FOR)" }, 
            { "106", "FOSTERHJEM_ETTER_§_4-27" }, 
            { "107", "BEREDSKAPSHJEM_UTENOM_FAMILIE_OG_NÆRE_NETTVERK" }, 
            { "108", "MIDLERTIDIG_HJEM_I_FAMILIE_OG_NÆRE_NETTVERK" }, 
            { "109", "ANDRE_FOSTERHJEMSTILTAK" }, 
            { "110", "MST_(MULTISYSTEMISK_TERAPI)" }, 
            { "111", "PMTO_(PARENT_MANAGEMENT_TRAINING_OREGON)" }, 
            { "112", "FFT_(FUNKSJONELL_FAMILIETERAPI)" }, 
            { "113", "WEBSTER-STRATTON_-DE_UTROLIGEÅRENE" }, 
            { "114", "ICDP_(INTERNATIONAL_CHILD_DEVELOPMENT_PROGRAM)" }, 
            { "115", "MARTE_MEO" }, 
            { "116", "ANDRE_HJEMMEBASERTE_TILTAK" }, 
            { "117", "SENTRE_FOR_FORELDRE_OG_BARN" }, 
            { "118", "VEDTAK_OM_RÅD_OG_VEILEDNING" }, 
            { "119", "HJEMMEKONSULENT/MILJØARBEIDER" }, 
            { "120", "ANDRE_TILTAK_FOR_Å_STYRKE_FORELDREFERDIGHETER" }, 
            { "122", "BARNEHAGE" }, 
            { "123", "SFO/AKTIVITETSSKOLE" }, 
            { "124", "FRITIDSAKTIVITETER" }, 
            { "125", "ØKONOMISK_HJELP_FOR_ØVRIG" }, 
            { "126", "BESØKSHJEM/AVLASTNINGSTILTAK" }, 
            { "127", "STØTTEKONTAKT" }, 
            { "128", "SAMTALEGRUPPER/_BARNEGRUPPER" }, 
            { "129", "UTDANNING_OG_ARBEID" }, 
            { "130", "ART_(AGGRESSION_REPLACEMENT_THERAPY)" }, 
            { "131", "ANDRE_TILTAK_FOR_Å_STYRKE_BARNETS_UTVIKLING" }, 
            { "132", "FRIVILLIG_TILSYN_I_HJEMMET" }, 
            { "133", "PÅLAGT_TILSYN_I_HJEMMET" }, 
            { "134", "TILSYN_UNDER_SAMVÆR" }, 
            { "135", "RUSKONTROLL" }, 
            { "136", "ANDRE_TILTAK_KNYTTET_TIL_TILSYN_OG_KONTROLL" }, 
            { "137", "FAMILIERÅD" }, 
            { "138", "NETTVERKSMØTER" }, 
            { "139", "INDIVIDUELL_PLAN" }, 
            { "140", "DELTAKELSE_I_ANSVARSGRUPPE/SAMARBEIDSTEAM" }, 
            { "141", "ANDRE_TILTAK_KNYTTET_TIL_NETTVERKSARBEID/SAMARBEID_MED_ANDRE_TJENESTER" }, 
            { "142", "MEDISINSK_UNDERSØKELSE_OG_BEHANDLING_(BARNEVERNLOVEN_§_4-10)" }, 
            { "143", "BEHANDLING_AV_BARN_MED_SÆRLIGE_OPPLÆRINGSBEHOV_(BARNEVERNSLOVEN_§_4-11)" }, 
            { "144", "PSYKISK_HELSEHJELP_FOR_BARN_OG_UNGE" }, 
            { "145", "ANDRE_TILTAK_KNYTTET_TIL_UNDERSØKELSE_OG_BEHANDLING_FRA_ANDRE_TJENESTER" }, 
            { "146", "ØKONOMISK_HJELP_VED_ETABLERING_I_EGEN_BOLIG/_HYBEL" }, 
            { "147", "BOLIG_MED_OPPFØLGING_(INKLUDERER_OGSÅ_BOFELLESSKAP)" }, 
            { "148", "BOTRENINGSKURS" }, { "149", "ANDRE_BOLIGTILTAK" } 
        };

        private readonly NameValueCollection Land = new() {
            { "0", "NO" },
            { "101", "DK" },
            { "102", "GL" },
            { "103", "FI" },
            { "104", "FO" },
            { "105", "IS" },
            { "106", "SE" },
            { "111", "AL" },
            { "112", "BE" },
            { "113", "BG" },
            { "114", "AD" },
            { "115", "EE" },
            { "117", "FR" },
            { "118", "GI" },
            { "119", "GR" },
            { "120", "BY" },
            { "121", "IE" },
            { "122", "HR" },
            { "123", "IT" },
            { "124", "LV" },
            { "125", "RS" },
            { "126", "MT" },
            { "127", "NL" },
            { "128", "LI" },
            { "129", "LU" },
            { "130", "MC" },
            { "131", "PL" },
            { "132", "PT" },
            { "133", "RO" },
            { "134", "SM" },
            { "136", "LT" },
            { "137", "ES" },
            { "138", "MD" },
            { "139", "GB" },
            { "140", "RU" },
            { "141", "CH" },
            { "143", "TR" },
            { "144", "DE" },
            { "146", "SI" },
            { "148", "UA" },
            { "152", "HU" },
            { "153", "AT" },
            { "154", "VA" },
            { "155", "BA" },
            { "156", "MK" },
            { "157", "SK" },
            { "158", "CZ" },
            { "159", "RS" },
            { "160", "ME" },
            { "161", "XK" },
            { "203", "DZ" },
            { "204", "AO" },
            { "205", "BW" },
            { "209", "SH" },
            { "213", "IO" },
            { "216", "BI" },
            { "220", "KM" },
            { "229", "BJ" },
            { "235", "GQ" },
            { "239", "CI" },
            { "241", "ER" },
            { "246", "ET" },
            { "249", "EG" },
            { "250", "DJ" },
            { "254", "GA" },
            { "256", "GM" },
            { "260", "GH" },
            { "264", "GN" },
            { "266", "GW" },
            { "270", "CM" },
            { "273", "CV" },
            { "276", "KE" },
            { "278", "CG" },
            { "279", "CD" },
            { "281", "LS" },
            { "283", "LR" },
            { "286", "LY" },
            { "289", "MG" },
            { "296", "MW" },
            { "299", "ML" },
            { "303", "MA" },
            { "304", "EH" },
            { "306", "MR" },
            { "307", "MU" },
            { "308", "NA" },
            { "309", "NE" },
            { "313", "NG" },
            { "319", "MZ" },
            { "322", "YT" },
            { "323", "RE" },
            { "326", "ZW" },
            { "329", "RW" },
            { "333", "ST" },
            { "336", "SN" },
            { "337", "CF" },
            { "338", "SC" },
            { "339", "SL" },
            { "346", "SO" },
            { "355", "SS" },
            { "356", "SD" },
            { "357", "SZ" },
            { "359", "ZA" },
            { "369", "TZ" },
            { "373", "TD" },
            { "376", "TG" },
            { "379", "TN" },
            { "386", "UG" },
            { "389", "ZM" },
            { "393", "BF" },
            { "404", "AF" },
            { "406", "AM" },
            { "407", "AZ" },
            { "409", "BH" },
            { "410", "BD" },
            { "412", "BT" },
            { "416", "BN" },
            { "420", "MM" },
            { "424", "LK" },
            { "426", "AE" },
            { "428", "PH" },
            { "430", "GE" },
            { "432", "TW" },
            { "436", "HK" },
            { "444", "IN" },
            { "448", "ID" },
            { "452", "IQ" },
            { "456", "IR" },
            { "460", "IL" },
            { "464", "JP" },
            { "476", "JO" },
            { "478", "KH" },
            { "480", "KZ" },
            { "484", "CN" },
            { "488", "KP" },
            { "492", "KR" },
            { "496", "KW" },
            { "500", "CY" },
            { "502", "KG" },
            { "504", "LA" },
            { "508", "LB" },
            { "510", "MO" },
            { "512", "MY" },
            { "513", "MV" },
            { "516", "MN" },
            { "520", "OM" },
            { "524", "PS" },
            { "528", "NP" },
            { "534", "PK" },
            { "537", "TL" },
            { "540", "QA" },
            { "544", "SA" },
            { "548", "SG" },
            { "550", "TJ" },
            { "552", "TM" },
            { "554", "UZ" },
            { "564", "SY" },
            { "568", "TH" },
            { "575", "VN" },
            { "578", "YE" },
            { "601", "VI" },
            { "602", "BB" },
            { "603", "AG" },
            { "604", "BZ" },
            { "605", "BS" },
            { "606", "BM" },
            { "608", "VG" },
            { "612", "CA" },
            { "613", "KY" },
            { "616", "CR" },
            { "620", "CU" },
            { "622", "DM" },
            { "624", "DO" },
            { "629", "GD" },
            { "631", "GP" },
            { "632", "GT" },
            { "636", "HT" },
            { "644", "HN" },
            { "648", "JM" },
            { "650", "MQ" },
            { "652", "MX" },
            { "654", "MS" },
            { "656", "BQ" },
            { "657", "AW" },
            { "660", "AI" },
            { "664", "NI" },
            { "668", "PA" },
            { "672", "SV" },
            { "676", "PM" },
            { "677", "KN" },
            { "678", "LC" },
            { "679", "VC" },
            { "680", "TT" },
            { "681", "TC" },
            { "684", "US" },
            { "685", "PR" },
            { "705", "AR" },
            { "710", "BO" },
            { "715", "BR" },
            { "720", "GY" },
            { "725", "CL" },
            { "730", "CO" },
            { "735", "EC" },
            { "740", "FK" },
            { "745", "GF" },
            { "755", "PY" },
            { "760", "PE" },
            { "765", "SR" },
            { "770", "UY" },
            { "775", "VE" },
            { "802", "AS" },
            { "805", "AU" },
            { "806", "SB" },
            { "807", "CX" },
            { "808", "CC" },
            { "809", "CK" },
            { "811", "FJ" },
            { "812", "VU" },
            { "813", "TO" },
            { "814", "PF" },
            { "815", "KI" },
            { "816", "TV" },
            { "817", "GU" },
            { "818", "NR" },
            { "819", "UM" },
            { "820", "NZ" },
            { "821", "NU" },
            { "822", "NF" },
            { "826", "FM" },
            { "827", "PG" },
            { "828", "PN" },
            { "829", "TK" },
            { "830", "WS" },
            { "832", "WF" },
            { "833", "NC" },
            { "835", "MH" },
            { "839", "PW" },
            { "840", "MP" }
        };

        private readonly NameValueCollection Hjemler = new() {
                { "1-1","Bvl._§_1-1" },
                { "1-2","Bvl._§_1-2" },
                { "1-3","Bvl._§_1-3" },
                { "1-3,2.","Bvl._§_1-3._2.ledd" },
                { "1-4","Bvl._§_1-4" },
                { "1-5","Bvl._§_1-5" },
                { "1-6","Bvl._§_1-6" },
                { "1-7","Bvl._§_1-7" },
                { "11","Fvl._§_11" },
                { "19a","Fvl._§_20" },
                { "2-1","Bvl._§_2-1" },
                { "2-5","Bvl._§_2-5" },
                { "3-1","Bvl._§_3-1" },
                { "3-2 a","Bvl._§_3-2._bokstav_a" },
                { "3-4","Bvl._§_3-4" },
                { "3-4.","Bvl._§_3-5" },
                { "3-5","Bvl._§_3-5" },
                { "4-1","Bvl._§_4-1" },
                { "4-10","Bvl._§_4-10" },
                { "4-11","Bvl._§_4-11" },
                { "4-12","Bvl._§_4-12" },
                { "4-12,1.a","Bvl._§_4-12._1.ledd._bokstav_a" },
                { "4-12,1.b","Bvl._§_4-12._1.ledd._bokstav_b" },
                { "4-12,1.c","Bvl._§_4-12._1.ledd._bokstav_c" },
                { "4-12,1.d","Bvl._§_4-12._1.ledd._bokstav_d" },
                { "4-12,2.","Bvl._§_4-12._2.ledd" },
                { "4-12,3.","Bvl._§_4-12._3.ledd" },
                { "4-13","Bvl._§_4-13" },
                { "4-14","Bvl._§_4-14" },
                { "4-14a","Bvl._§_4-14._bokstav_a" },
                { "4-14b","Bvl._§_4-14._bokstav_b" },
                { "4-14c","Bvl._§_4-14._bokstav_c" },
                { "4-15","Bvl._§_4-15" },
                { "4-15,1.","Bvl._§_4-15._1.ledd" },
                { "4-15,2.","Bvl._§_4-15._2.ledd" },
                { "4-15,3.","Bvl._§_4-15._3.ledd" },
                { "4-15,4.","Bvl._§_4-15._4.ledd" },
                { "4-16","Bvl._§_4-16" },
                { "4-17","Bvl._§_4-17" },
                { "4-18","Bvl._§_4-18" },
                { "4-18,1.","Bvl._§_4-18._1.ledd" },
                { "4-18,2.","Bvl._§_4-18._2.ledd" },
                { "4-19","Bvl._§_4-19" },
                { "4-19,1.","Bvl._§_4-19._1.ledd" },
                { "4-19,2.","Bvl._§_4-19._2.ledd" },
                { "4-19,3.","Bvl._§_4-19._3.ledd" },
                { "4-19,4.","Bvl._§_4-19._4.ledd" },
                { "4-19,4.a","Bvl._§_4-19._4.ledd._bokstav_a" },
                { "4-19,4.b","Bvl._§_4-19._4.ledd._bokstav_b" },
                { "4-19,5.","Bvl._§_4-19._5.ledd" },
                { "4-2","Bvl._§_4-2" },
                { "4-20","Bvl._§_4-20" },
                { "4-20,1.","Bvl._§_4-20._1.ledd" },
                { "4-20,2.","Bvl._§_4-20._2.ledd" },
                { "4-20,3.","Bvl._§_4-20._3.ledd" },
                { "4-20,3.a","Bvl._§_4-20._3.ledd._bokstav_a" },
                { "4-20,3.b","Bvl._§_4-20._3.ledd._bokstav_b" },
                { "4-20,3.c","Bvl._§_4-20._3.ledd._bokstav_c" },
                { "4-20,3.d","Bvl._§_4-20._3.ledd._bokstav_d" },
                { "4-20,4","Bvl._§_4-20._4.ledd" },
                { "4-20,4.","Bvl._§_4-20._4.ledd" },
                { "4-20a","Bvl._§_4-20._bokstav_a" },
                { "4-21","Bvl._§_4-21" },
                { "4-21,1.","Bvl._§_4-21._1.ledd" },
                { "4-21,2.","Bvl._§_4-21._2.ledd" },
                { "4-22","Bvl._§_4-22" },
                { "4-22,1.a","Bvl._§_4-22._1.ledd._bokstav_a" },
                { "4-22,1.b","Bvl._§_4-22._1.ledd._bokstav_b" },
                { "4-22,2.","Bvl._§_4-22._2.ledd" },
                { "4-22,3.","Bvl._§_4-22._3.ledd" },
                { "4-22,4.","Bvl._§_4-22._4.ledd" },
                { "4-22,5.","Bvl._§_4-22._5.ledd" },
                { "4-24","Bvl._§_4-24" },
                { "4-24,1.","Bvl._§_4-24._1.ledd" },
                { "4-24,2.","Bvl._§_4-24._2.ledd" },
                { "4-24,3.","Bvl._§_4-24._3.ledd" },
                { "4-24,4.","Bvl._§_4-24._4.ledd" },
                { "4-25","Bvl._§_4-25" },
                { "4-25,1.","Bvl._§_4-25._1.ledd" },
                { "4-25,2.1","Bvl._§_4-25._2.ledd._1.punktum" },
                { "4-25,2.2","Bvl._§_4-25._2.ledd._2.punktum" },
                { "4-25,2.3","Bvl._§_4-25._2.ledd._3.punktum" },
                { "4-25,3.","Bvl._§_4-25._3.ledd" },
                { "4-26","Bvl._§_4-26" },
                { "4-26,1.","Bvl._§_4-26._1.ledd" },
                { "4-26,2.","Bvl._§_4-26._2.ledd" },
                { "4-26,3.","Bvl._§_4-26._3.ledd" },
                { "4-26,4.","Bvl._§_4-26._4.ledd" },
                { "4-27","Bvl._§_4-27" },
                { "4-28","Bvl._§_4-28" },
                { "4-28,1.","Bvl._§_4-28._1.ledd" },
                { "4-28,2.","Bvl._§_4-28._2.ledd" },
                { "4-28,3.","Bvl._§_4-28._3.ledd" },
                { "4-28,4.","Bvl._§_4-28._4.ledd" },
                { "4-29","Bvl._§_4-29" },
                { "4-29,1.","Bvl._§_4-29._1.ledd" },
                { "4-29,2.","Bvl._§_4-29._2.ledd" },
                { "4-29,3.","Bvl._§_4-29._3.ledd" },
                { "4-29,4.","Bvl._§_4-29._4.ledd" },
                { "4-29,5.","Bvl._§_4-29._5.ledd" },
                { "4-29,6.","Bvl._§_4-29._6.ledd" },
                { "4-29,7.","Bvl._§_4-29._7.ledd" },
                { "4-3","Bvl._§_4-3" },
                { "4-3,1.","Bvl._§_4-3._1.ledd" },
                { "4-3,2.","Bvl._§_4-3._2.ledd" },
                { "4-3,3.","Bvl._§_4-3._3.ledd" },
                { "4-3,4.","Bvl._§_4-3._4.ledd" },
                { "4-3,5.","Bvl._§_4-3._5.ledd" },
                { "4-30","Bvl._§_4-30" },
                { "4-4","Bvl._§_4-4" },
                { "4-4,1.","Bvl._§_4-4._1.ledd" },
                { "4-4,2.","Bvl._§_4-4._2.ledd" },
                { "4-4,3.","Bvl._§_4-4._3.ledd" },
                { "4-4,3.1","Bvl._§_4-4._3.ledd._1.punktum" },
                { "4-4,3.2","Bvl._§_4-4._3.ledd._2.punktum" },
                { "4-4,3.3","Bvl._§_4-4._3.ledd._3.punktum" },
                { "4-4,4.","Bvl._§_4-4._4.ledd" },
                { "4-4,5.","Bvl._§_4-4._5.ledd" },
                { "4-4,6.","Bvl._§_4-4._6.ledd" },
                { "4-4a","Bvl._§_4-4._bokstav_a" },
                { "4-4a,1.","Bvl._§_4-4._bokstav_a._1.ledd" },
                { "4-4a,2.","Bvl._§_4-4._bokstav_a._2.ledd" },
                { "4-4a,3.","Bvl._§_4-4._bokstav_a._3.ledd​"},
                { "4-5","Bvl._§_4-5" },
                { "4-6","Bvl._§_4-6" },
                { "4-6,1.","Bvl._§_4-6._1.ledd" },
                { "4-6,2.","Bvl._§_4-6._2.ledd" },
                { "4-6,3.","Bvl._§_4-6._3.ledd" },
                { "4-6,4.","Bvl._§_4-6._4.ledd" },
                { "4-6,5.","Bvl._§_4-6._5.ledd" },
                { "4-7","Bvl._§_4-7" },
                { "4-7,1.","Bvl._§_4-7._1.ledd" },
                { "4-7,2.","Bvl._§_4-7._2.ledd" },
                { "4-7,3.","Bvl._§_4-7._3.ledd" },
                { "4-8","Bvl._§_4-8" },
                { "4-8,1.","Bvl._§_4-8._1.ledd" },
                { "4-8,2.","Bvl._§_4-8._2.ledd" },
                { "4-8,2.2","Bvl._§_4-8._2.ledd._2.punktum" },
                { "4-8,3.","Bvl._§_4-8._3.ledd" },
                { "4-9","Bvl._§_4-9" },
                { "4-9,1.","Bvl._§_4-9._1.ledd" },
                { "4-9,2.","Bvl._§_4-9._2.ledd" },
                { "4-9,3.","Bvl._§_4-9._3.ledd" },
                { "4-9,4.","Bvl._§_4-9._4.ledd" },
                { "43a","Bl._§_43._bokstav_a" },
                { "6-1","Bvl._§_6-1" },
                { "6-1,1.","Bvl._§_6-1" },
                { "6-1,2.","Bvl._§_6-1" },
                { "6-1,3.","Bvl._§_6-1" },
                { "6-1,4.","Bvl._§_6-1" },
                { "6-10","Bvl._§_6-10" },
                { "6-10,1.","Bvl._§_6-10" },
                { "6-10,2.","Bvl._§_6-10" },
                { "6-10,3.","Bvl._§_6-10" },
                { "6-10,4.","Bvl._§_6-10" },
                { "6-10,5.","Bvl._§_6-10" },
                { "6-10,6.","Bvl._§_6-10" },
                { "6-10,7.","Bvl._§_6-10" },
                { "6-2","Bvl._§_6-2" },
                { "6-3","Bvl._§_6-3" },
                { "6-3,1.","Bvl._§_6-3" },
                { "6-3,2.","Bvl._§_6-3" },
                { "6-3a","Bvl._§_6-3._bokstav_a" },
                { "6-4","Bvl._§_6-4" },
                { "6-4,1.","Bvl._§_6-4._1.ledd" },
                { "6-4,2.","Bvl._§_6-4._2.ledd" },
                { "6-4,3.","Bvl._§_6-4._3.ledd" },
                { "6-5","Bvl._§_6-5" },
                { "6-5,1.","Bvl._§_6-5" },
                { "6-5,2.","Bvl._§_6-5" },
                { "6-6","Bvl._§_6-6" },
                { "6-6,1.","Bvl._§_6-6" },
                { "6-6,2.","Bvl._§_6-6" },
                { "6-7","Bvl._§_6-7" },
                { "6-7a","Bvl._§_6-7._bokstav_a" },
                { "6-8","Bvl._§_6-8" },
                { "6-9","Bvl._§_6-9" },
                { "6-9,1.","Bvl._§_6-9" },
                { "6-9,2.","Bvl._§_6-9" },
                { "6-9,3.","Bvl._§_6-9" },
                { "6-9,4.","Bvl._§_6-9" },
                { "7-10","Bvl._§_7-10" },
                { "7-11","Bvl._§_7-11" },
                { "7-2","Bvl._§_7-22" },
                { "7-22","Bvl._§_7-22" },
                { "7-23","Bvl._§_7-23" },
                { "7-24","Bvl._§_7-24" },
                { "7-25","Bvl._§_7-25" },
                { "7-4","Bvl._§_7-4" },
                { "7-4,1.","Bvl._§_7-4" },
                { "7-4,2.","Bvl._§_7-4" },
                { "8-1","Bvl._§_8-1" },
                { "8-3","Bvl._§_8-3" },
                { "8-4","Bvl._§_8-4" },
                { "8-4,1.","Bvl._§_8-4" },
                { "8-4,2.","Bvl._§_8-4" },
                { "8-5","Bvl._§_8-5" },
                { "9-1","Bvl._§_9-1" },
                { "9-1,1.","Bvl._§_9-1" },
                { "9-1,2.","Bvl._§_9-1" },
                { "9-2","Bvl._§_9-2" },
                { "9-2,1.","Bvl._§_9-2" },
                { "9-2,2.","Bvl._§_9-2" },
                { "9-2,3.","Bvl._§_9-2" },
                { "9-2,4.","Bvl._§_9-2" },
                { "9-2,5.","Bvl._§_9-2" },
                { "9-2,6.","Bvl._§_9-2" },
                { "9-3","Bvl._§_9-1" },
                { "9-3,1.","Bvl._§_9-0" },
                { "9-3,2.","Bvl._§_9-1" },
                { "9-3,3.","Bvl._§_9-2" },
                { "9-3,4.","Bvl._§_9-3" },
                { "fvl 2,1e","Fvl._§_2._bokstav_e" },
                { "fvl. 19","Fvl._§_19" },
                { "jf. § 3-2a","Bvl._§_3-2._bokstav_a" },
                { "jf. § 4-15, 3. ledd","Bvl._§_4-15._3.ledd" },
                { "jf. § 4-15, 4. ledd","Bvl._§_4-15._4.ledd" },
                { "jf. § 4-28","Bvl._§_4-28" },
                { "jf. § 4-5","Bvl._§_4-5" },
                //TODO Hjemler - Legg inn hjemler for ny barnevernlov når de er lagt inn i Modulus Barn
                { "§ 1-2","" },
                { "§ 1-3","" },
                { "§ 1-4","" },
                { "§ 2-2","" },
                { "§ 2-3","" },
                { "§ 2-4","" },
                { "§ 2-5","" },
                { "§ 2-5,1.","" },
                { "§ 2-5,2.","" },
                { "§ 2-5,3.","" },
                { "§ 3-1","" },
                { "§ 3-2","" },
                { "§ 3-3","" },
                { "§ 3-4","" },
                { "§ 3-5","" },
                { "§ 3-6","" },
                { "§ 3-7","" },
                { "§ 3-8","" },
                { "§ 4-1","" },
                { "§ 4-2","" },
                { "§ 4-2,1.","" },
                { "§ 4-2,2.","" },
                { "§ 4-2,3.","" },
                { "§ 4-2,4.","" },
                { "§ 4-2,5.","" },
                { "§ 4-3","" },
                { "§ 4-4","" },
                { "§ 4-5","" },
                { "§ 5-1","" },
                { "§ 5-1. b","" },
                { "§ 5-1. c","" },
                { "§ 5-1. d","" },
                { "§ 5-1. e","" },
                { "§ 5-1. f","" },
                { "§ 5-1. g","" },
                { "§ 5-1.a","" },
                { "§ 5-2","" },
                { "§ 5-3","" },
                { "§ 5-4","" },
                { "§ 5-5","" },
                { "§ 5-6","" },
                { "§ 5-7","" },
                { "§ 5-8","" },
                { "§ 6-1","" },
                { "§ 6-2","" },
                { "§ 6-2,1.","" },
                { "§ 6-2,2.","" },
                { "§ 6-2,3.","" },
                { "§ 6-2,5.","" },
                { "§ 6-2,6.","" },
                { "§ 6-4,4.","" },
                { "§ 6-6","" },
                { "§ 7-1","" },
                { "§ 7-2","" },
                { "§ 7-2,1.","" },
                { "§ 7-2,2.","" },
                { "§ 7-2,3.","" },
                { "§ 7-2,4.","" },
                { "§ 7-2,5.","" },
                { "§ 7-3","" },
                { "§ 7-3,1.","" },
                { "§ 7-3,2.","" },
                { "§ 8-1","" },
                { "§ 9-1","" },
                { "§ 9-10","" },
                { "§ 9-11","" },
                { "§ 9-2","" },
                { "§ 9-3","" },
                { "§ 9-4","" },
                { "§ 9-5","" },
                { "§ 9-6","" },
                { "§ 9-8","" }
        };

        private readonly NameValueCollection Årsakskoder = new() {
            { "01","01_VANSKELIG_Å FÅ TAK_I_SAKKYNDIG_BISTAND" },
            { "02","02_SAKKYNDIG_BRUKER_LENGRE_TID_ENN_FORVENTET" },
            { "03","03_BEHOV_FOR_UTREDNING_FORELDRE_BARN" },
            { "04","04_BEHOV_FOR_SAKKYNDIG" },
            { "05","05_BEHOV_FOR_JURIDISK_BISTAND_I_BARNEVERNSTJENESTEN" },
            { "06","06_BEHOV_FOR_JURIDISK_BISTAND_TIL_PARTENE_(FORELDRE_BARN)" },
            { "07","07_SAK_TIL_FYLKESNEMNDA" },
            { "08","08_KOMPLISERT_SAMMENSATT_SAK" },
            { "09","09_MISTANKE_OM_SEKSUELLE_OVERGREP" },
            { "10","10_FLERKULTURELL_PROBLEMATIKK" },
            { "11","11_MANGLENDE_KAPASITET_I_TOLKETJENESTEN" },
            { "12","12_ULIKE_PROBLEMER_VEDR_TVERRETATLIG_SAMARBEID" },
            { "13","13_MANGLENDE_KAPASITET_FERIE_SYKDOM_HOS_TVERRFAGLIG_SAMARBEIDSPARTNER" },
            { "14","14_FORELDRE_BARN_UNNDRAR_SEG_KONTAKT_MED_BARNEVERNSTJENESTEN" },
            { "15","15_VANSKELIG_Å FÅ TAK_I_FAMILIE_BARN" },
            { "16","16_TOK_TID_Å ETABLERE_SAMARBEID_SKAPE_TILLIT" },
            { "17","17_FORELDRE_BARN_PÅ FERIE" },
            { "18","18_SYKDOM_FAMILIE_BARN" },
            { "19","19_TOK_TID_Å OPPNÅ SAMTYKKE_TIL_HJELPETILTAK" },
            { "20","20_MANGLENDE_KAPASITET_I_BARNEVERNSTJENESTEN" },
            { "21","21_VAKANTE_STILLINGER_I_BARNEVERNSTJENESTEN" },
            { "22","22_SYKDOM_I_BARNEVERNSTJENESTEN" },
            { "23","23_FERIEAVVIKLING_I_BARNEVERNSTJENESTEN" },
            { "24","24_NYANSATTE_OPPLÆRING" },
            { "25","25_REDUKSJON_I_STILLINGSHJEMLER" },
            { "26","26_PROBLEMER_VEDRØRENDE_OVERFØRING_AV_SAK" },
            { "27","27_LØPENDE_HELLIDAGER_(JUL_PÅSKE_ETC)" },
            { "30","30_ANNET_(BRUK_GJERNE_STIKKORD)" }
        };

        private readonly NameValueCollection BydelConnectionStrings = new() {
            { "BAL", "Server=*;Database=BAL_BV12P;Trusted_Connection=True;Encrypt=False;" },
            { "BBJ", "Server=*;Database=BBJ_BV09P;Trusted_Connection=True;Encrypt=False;" },
            { "BFR", "Server=*;Database=BFR_BV05P;Trusted_Connection=True;Encrypt=False;" },
            { "BGO", "Server=*;Database=BGO_BV01P;Trusted_Connection=True;Encrypt=False;" },
            { "BGR", "Server=*;Database=BGR_BV10P;Trusted_Connection=True;Encrypt=False;" },
            { "BGA", "Server=*;Database=BGA_BV02P;Trusted_Connection=True;Encrypt=False;" },
            { "BNA", "Server=*;Database=BNA_BV08P;Trusted_Connection=True;Encrypt=False;" },
            { "BNS", "Server=*;Database=BNS_BV14P;Trusted_Connection=True;Encrypt=False;" },
            { "BSA", "Server=*;Database=BSA_BV03P;Trusted_Connection=True;Encrypt=False;" },
            { "BSH", "Server=*;Database=BSH_BV04P;Trusted_Connection=True;Encrypt=False;" },
            { "BSR", "Server=*;Database=BSR_BV11P;Trusted_Connection=True;Encrypt=False;" },
            { "BSN", "Server=*;Database=BSN_BV15P;Trusted_Connection=True;Encrypt=False;" },
            { "BUN", "Server=*;Database=BUN_BV06P;Trusted_Connection=True;Encrypt=False;" },
            { "BVA", "Server=*;Database=BVA_BV07P;Trusted_Connection=True;Encrypt=False;" },
            { "BOS", "Server=*;Database=BOS_BV13P;Trusted_Connection=True;Encrypt=False;" },
            { "DEM", "Server=*;Database=HEL_BVDEMO;Trusted_Connection=True;Encrypt=False;" },
            { "BVV", "Server=*;Database=BVV;Trusted_Connection=True;Encrypt=False;" },
            { "MIG", "Server=*;Database=Migrering;Trusted_Connection=True;Encrypt=False;" }
        };

        //TODO Mappings - BydelHovedsaksbehandlere - Legg inn hovedsaksbehandlere
        private readonly NameValueCollection BydelHovedsaksbehandlere = new() {
            { "BAL", "MEKJ" },
            { "BBJ", "BVKM" },
            { "BFR", "BVMD" },
            { "BGO", "TUJA" },
            { "BGR", "LENE" },
            { "BGA", "ARTA" },
            { "BNA", "BVAI" },
            { "BNS", "KNHA" },
            { "BSA", "BVAI" },
            { "BSH", "BVAI" },
            { "BSR", "MLSK" },
            { "BSN", "CAGR" },
            { "BUN", "SAMO" },
            { "BVA", "HEVA" },
            { "BOS", "CETO" },
            { "DEM", "ANJN" }
        };

        private readonly List<string> Bydeler = new()
        {
            "BAL",
            "BBJ",
            "BFR",
            "BGO",
            "BGR",
            "BGA",
            "BNA",
            "BNS",
            "BSA",
            "BSH",
            "BSR",
            "BSN",
            "BUN",
            "BVA",
            "BOS"
        };

        private readonly NameValueCollection BVVTypeBarnevernsvaktsaker = new() {
            { "5843", "BARNETS_RUSMISBRUK" },
            { "5844", "FORELDRENES_RUSMISBRUK" },
            { "5845", "BARNETS_KRIMINALITET" },
            { "5846", "FORELDRENES_KRIMINALITET" },
            { "5847", "SAMVÆRSKONFLIKT" },
            { "5848", "BARNET_UTSATT_FOR_SEKSUELLE_OVERGREP" },
            { "5849", "VOLD_I_HJEMMET_BARNET_VITNE_TIL_VOLD_I_NÆRE_RELASJONER" },
            { "5850", "ANDRE_FORHOLD_VED_BARNETS_SITUASJON" },
            { "5851", "FORELDRENES_PSYKISKE_PROBLEM_LIDELSE" },
            { "5852", "BARNETS_ADFERD" },
            { "5853", "BARNETS_PSYKISKE_PROBLEM_LIDELSE" },
            { "5854", "KONFLIKT_MELLOM_BARN_UNGE" }
        };

        private readonly NameValueCollection BVVHovedkategorier = new() {
            { "5855", "FORELSRENES_SOMATISKE_SYKDOM" },
            { "5856", "FORELDRENES_PSYKISKE_PROBLEM_LIDELSE" },
            { "5857", "FORELDRENES_RUSMISBRUK" },
            { "5858", "FORELDRENES_MANGLENDE_FORELDREFERDIGHETER" },
            { "5859", "FORELDRENES_KRIMINALITET" },
            { "5860", "HØY_GRAD_AV_KONFLIKT_I_HJEMMET" },
            { "5861", "VOLD_I_HJEMMET_BARNET_VITNE_TIL_VOLD_I_NÆRE_RELASJONER" },
            { "5862", "BARNET_UTSATT_FOR_VANSKJØTSEL" },
            { "5863", "BARNET_UTSATT_FOR_FYSISK_MISHANDLING" },
            { "5864", "BARNET_UTSATT_FOR_PSYKISK_MISHANDLING" },
            { "5865", "BARNET_UTSATT_FOR_SEKSUELLE_OVERGREP" },
            { "5866", "BARNET_MANGLER_OMSORGSPERSON" },
            { "5867", "BARNET_HAR_NEDSATT_FUNKSJONSEVNE" },
            { "5868", "BARNETS_PSYKISKE_PROBLEM_LIDELSE" },
            { "5869", "BARNETS_RUSMISBRUK" },
            { "5870", "BARNETS_ADFERD" },
            { "5871", "BARNETS_RELASJONSVANSKER" },
            { "5872", "ANDRE_FORHOLD_VED_FORELDRENE_FAMILIEN" },
            { "5873", "ANDRE_FORHOLD_VED_BARNETS_SITUASJON" },
            { "6078", "SAVNET_BARN" },
            { "6079", "UTEBLIVELSE_FRA_HJEMMET_INSTITUSJON" },
            { "7190", "ANNET" },
            { "11280", "BARNETS_ADFERD" },
            { "11281", "BARNETS_KRIMINALITET" },
            { "11282", "SAMVÆRSKONFLIKT" },
            { "11283", "KONFLIKT_MELLOM_BARN_UNGE" },
            { "11284", "KATASTROFE_KRISEHJELP" }
        };

        private readonly NameValueCollection BVVMeldere = new() {
            { "5874", "BARNET_SELV" },
            { "5875", "MOR_FAR_FORESATTE" },
            { "5876", "FAMILIE_FOR_ØVRIG" },
            { "5877", "ANDRE_PRIVATPERSONER" },
            { "5878", "BARNEVERNTJENESTEN" },
            { "5879", "NAV_KOMMUNE_OG_STAT" },
            { "5880", "BARNEVERNVAKT" },
            { "5881", "POLITI_LENSMANN" },
            { "5882", "BARNEHAGE" },
            { "5883", "HELSESTASJON_SKOLEHELSETJ" },
            { "5884", "SKOLE" },
            { "5885", "PPT_PSYKISK_HELSEVERN_BARN_UNG_VOKSNE" },
            { "5886", "PSYKISK_HELSEVERN_BARN_OG_UNGE" },
            { "5887", "PSYKISK_HELSEVERN_VOKSNE" },
            { "5888", "LEGE_TANNLEGE" },
            { "5889", "FAMILIEVERNKONTOR" },
            { "5890", "TJENESTER_FOR_OPPFØLG" },
            { "5891", "KRISESENTER" },
            { "5892", "ASYLMOTTAK_UDI_INNVMYNDIGHET" },
            { "5893", "UTEKONTAKT_FRITIDSKLUBB" },
            { "5894", "FRIVILLIGE_ORG_IDRETTSLAG" },
            { "5895", "ANDRE_OFFENTLIGE_INSTANSER" },
            { "5896", "ANDRE" },
            { "6080", "UTESEKSJONEN" },
            { "6511", "INSTITUSJON" },
            { "7258", "FOSTERHJEM_BEREDSKAPSHJEM" },
            { "7290", "ADVOKAT" },
            { "11278", "BARNEVERNSINSTITUSJON" },
            { "11279", "AMK_LEGEVAKT_SYKEHUS" },
            { "11987", "POLITI_LENSMANN" }
        };

        private readonly NameValueCollection BVVJournalCategories = new() {
            { "6048", "HJEMMEBESØK" },
            { "6049", "TELEFONSAMTALE" },
            { "6050", "SMS_MELDING" },
            { "6051", "NOTAT" },
            { "6052", "SAMTALE" },
            { "6053", "SAMARBEIDSMØTE" },
            { "6054", "KONTROLLBESØK" },
            { "6575", "SAVNETSKJEMA" },
            { "6661", "MØTE_PÅ_KONTORET" },
            { "7189", "EPOST" },
            { "8367", "SØK" }
        };

        private readonly NameValueCollection BVVCorrespondenceCategories = new() {
            { "5842", "ANNEN_DOKUMENTASJON" },
            { "6055", "EPOST" },
            { "6056", "BEKYMRINGSMELDING" },
            { "6057", "KONTROLLBESØK" },
            { "6058", "KONTROLLBESØK" },
            { "6059", "INFORMASJON_TIL_BARNEVERNTJENESTEN" },
            { "6060", "VEDTAK_AKUTT" }
        };

        private readonly NameValueCollection BVVUsernameCache = new();

        private List<Sokrates> SokratesList;

        private readonly NameValueCollection Bydelskontorer = new() {
            { "1", "BFR" },
            { "2", "BGO" },
            { "3", "BGO" },
            { "4", "BSH" },
            { "5", "BSH" },
            { "6", "BGA" },
            { "7", "BFR" },
            { "8", "BVA" },
            { "9", "BNA" },
            { "10", "BGR" },
            { "11", "BBJ" },
            { "12", "BGA" },
            { "13", "BAL" },
            { "14", "BOS" },
            { "15", "BNS" },
            { "16", "BSR" },
            { "17", "BGR" },
            { "18", "BOS" },
            { "19", "BBJ" },
            { "20", "BAL" },
            { "21", "BGA" },
            { "22", "BVA" },
            { "23", "BUN" },
            { "24", "BNS" },
            { "25", "BSN" },
            { "26", "BNS" },
            { "27", "BSN" },
            { "28", "BNS" },
            { "29", "BSR" },
            { "30", "BAL" },
            { "31", "BSA" },
            { "32", "BNA" },
            { "33", "BOS" },
            { "82", "BAL" },
            { "83", "BAL" },
            { "84", "BAL" },
            { "85", "BSH" },
            { "105", "BGA" },
            { "106", "BSN" },
            { "107", "BSR" }
        };
        #endregion

        #region Contructors
        public Mappings(bool useSokrates)
        {
            UseSokrates = useSokrates;
            SokratesList = new List<Sokrates>();
        }
        #endregion

        #region Lovhjemler
        public string GetModulusLovhjemmel(string familiaLovhjemmel)
        {
            if (!string.IsNullOrEmpty(familiaLovhjemmel))
            {
                familiaLovhjemmel = familiaLovhjemmel.Trim();
            }
            return Hjemler[familiaLovhjemmel];
        }
        #endregion

        #region SSB
        public string GetSSBHovedkategori(string tiltakstype)
        {
            tiltakstype = tiltakstype?.Trim();
            return SSBHovedkategori[tiltakstype];
        }

        public string GetSSBUnderkategori(string tiltakstype)
        {
            tiltakstype = tiltakstype?.Trim();
            return SSBUnderkategori[tiltakstype];
        }
        #endregion

        #region Land
        public string GetLand(decimal? landkode)
        {
            if (!landkode.HasValue)
            {
                return null;
            }
            return Land[((int)landkode).ToString()];
        }
        #endregion

        #region Bydeler og Connection strings
        public string GetConnectionstring(string bydel, string servername)
        {
            return BydelConnectionStrings[bydel].Replace("*", servername);
        }
        public List<string> GetAlleBydeler()
        {
            return Bydeler;
        }
        #endregion

        #region Hovedsaksbehandler bydel
        public string GetHovedsaksbehandlerBydel(string bydel)
        {
            return BydelHovedsaksbehandlere[bydel];
        }
        #endregion

        #region Årsakskoder
        public string GetÅrsakskodeUtvidelseFrist(string kode)
        {
            return Årsakskoder[kode];
        }
        #endregion

        #region Sokrates
        public void ResetSokratesList()
        {
            SokratesList = new List<Sokrates>();
        }
        public void AddKlient(Sokrates sokrates)
        {
            SokratesList.Add(sokrates);
        }
        public bool IsOwner(decimal kliLoepenr)
        {
            if (!UseSokrates)
            {
                return true;
            }
            return SokratesList.Find(s => s.kliLoepenr == kliLoepenr).eierBydel;
        }
        public List<TidligereAvdeling> GetTidligereBydeler(decimal kliLoepenr)
        {
            if (!UseSokrates)
            {
                return null;
            }
            return SokratesList.Find(s => s.kliLoepenr == kliLoepenr).tidligereAvdelinger;
        }
        public bool HarTidligereBydeler(decimal kliLoepenr)
        {
            if (!UseSokrates)
            {
                return false;
            }
            return SokratesList.Find(s => s.kliLoepenr == kliLoepenr).tidligereAvdelinger.Count > 0;
        }
        public List<Sokrates> GetSokratesList()
        {
            return SokratesList;
        }
        public bool IsKontoretDenneBydel(int officeId, string bydel)
        {
            if (Bydelskontorer[officeId.ToString()] == bydel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetBydelFraOffice(int officeId)
        {
            return Bydelskontorer[officeId.ToString()];
        }
        #endregion

        #region Visma Flyt Barnevernvakt
        public static string GetBVVRolleSak(int familyroleRegistryId)
        {
            string rolle = "";

            switch (familyroleRegistryId)
            {
                case 1:
                    rolle = "MOR";
                    break;
                case 2:
                    rolle = "FAR";
                    break;
                case 3:
                case 4:
                    rolle = "SØSKEN";
                    break;
                case 6020:
                case 6021:
                    rolle = "FOSTERHJEM";
                    break;
                case 6061:
                case 6062:
                    rolle = "ØVRIG_FAMILIE_SLEKT";
                    break;
                case 6063:
                case 6064:
                case 6065:
                case 6066:
                    rolle = "BESTEFORELDER";
                    break;
                case 7500:
                    rolle = "STEFAR";
                    break;
                case 7501:
                    rolle = "STEMOR";
                    break;
                case 6067:
                    rolle = "BARNEVERNTJENESTE";
                    break;
                case 6068:
                    rolle = "MILJØARBEIDER";
                    break;
                case 6069:
                    rolle = "SKOLE";
                    break;
                case 6071:
                    rolle = "FAMILIERÅDSKOORDINATOR";
                    break;
                case 6073:
                    rolle = "ANNEN";
                    break;
                case 6652:
                    rolle = "VERGE";
                    break;
            }
            return rolle;
        }
        public static string GetBVVRelasjonSak(int familyroleRegistryId)
        {
            string relasjon = "";

            switch (familyroleRegistryId)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 6061:
                case 6062:
                case 6063:
                case 6064:
                case 6065:
                case 6066:
                case 7500:
                case 7501:
                    relasjon = "FAMILIE";
                    break;
                case 6020:
                case 6021:
                case 6067:
                case 6068:
                case 6069:
                case 6071:
                    relasjon = "PROFESJONELL_KONTAKT";
                    break;
                case 6073:
                case 6652:
                    relasjon = "ANNEN";
                    break;
            }
            return relasjon;
        }
        public string GetBVVTypeBarnevernsvaktsak(int? personcaseTypeRegistryId)
        {
            string typeBarnevernsvaktsak = null;

            if (personcaseTypeRegistryId.HasValue)
            {
                return BVVTypeBarnevernsvaktsaker[personcaseTypeRegistryId.ToString()];
            }
            return typeBarnevernsvaktsak;
        }
        public string GetBVVHovedkategori(int? mainCategoryRegistryId)
        {
            string hovedkategori = null;

            if (mainCategoryRegistryId.HasValue)
            {
                return BVVHovedkategorier[mainCategoryRegistryId.ToString()];
            }
            return hovedkategori;
        }
        public string GetBVVMelder(int? reporterTypeRegistryId)
        {
            string melder = null;

            if (reporterTypeRegistryId.HasValue)
            {
                return BVVMeldere[reporterTypeRegistryId.ToString()];
            }
            return melder;
        }
        public string GetBVVJournalCategory(int journalCategoryRegistryId)
        {
            return BVVJournalCategories[journalCategoryRegistryId.ToString()];
        }
        public string GetBVVCorrespondenceCategory(int correspondenceCategoryRegistryId)
        {
            return BVVCorrespondenceCategories[correspondenceCategoryRegistryId.ToString()];
        }
        public void ClearBVVUsernameCache()
        {
            BVVUsernameCache.Clear();
        }
        public void AddBVVUsernameCache(int employeeId, string userName)
        {
            BVVUsernameCache.Add(employeeId.ToString(), userName);
        }
        public string GetBVVUsername(int employeeId)
        {
            return BVVUsernameCache[employeeId.ToString()];
        }
        #endregion
    }
}
