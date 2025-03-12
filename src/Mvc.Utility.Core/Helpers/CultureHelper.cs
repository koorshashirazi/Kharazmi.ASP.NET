using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Mvc.Utility.Core.Helpers
{
    public sealed class SpecificCultureType
    {
        private SpecificCultureType(string key)
        {
            Key = key;
        }

        public string Key { get; set; }

        public static SpecificCultureType Afar_Ethiopia => new SpecificCultureType("aa-ET");

        public static SpecificCultureType Afrikaans => new SpecificCultureType("af");

        public static SpecificCultureType Afrikaans_Namibia => new SpecificCultureType("af-NA");

        public static SpecificCultureType Afrikaans_South_Africa => new SpecificCultureType("af-ZA");

        public static SpecificCultureType Aghem => new SpecificCultureType("agq");

        public static SpecificCultureType Aghem_Cameroon => new SpecificCultureType("agq-CM");

        public static SpecificCultureType Akan => new SpecificCultureType("ak");

        public static SpecificCultureType Akan_Ghana => new SpecificCultureType("ak-GH");

        public static SpecificCultureType Amharic => new SpecificCultureType("am");

        public static SpecificCultureType Amharic_Ethiopia => new SpecificCultureType("am-ET");

        public static SpecificCultureType Arabic => new SpecificCultureType("ar");

        public static SpecificCultureType Arabic_World => new SpecificCultureType("ar-001");

        public static SpecificCultureType Arabic_United_Arab_Emirates => new SpecificCultureType("ar-AE");

        public static SpecificCultureType Arabic_Bahrain => new SpecificCultureType("ar-BH");

        public static SpecificCultureType Arabic_Djibouti => new SpecificCultureType("ar-DJ");

        public static SpecificCultureType Arabic_Algeria => new SpecificCultureType("ar-DZ");

        public static SpecificCultureType Arabic_Egypt => new SpecificCultureType("ar-EG");

        public static SpecificCultureType Arabic_Eritrea => new SpecificCultureType("ar-ER");

        public static SpecificCultureType Arabic_Israel => new SpecificCultureType("ar-IL");

        public static SpecificCultureType Arabic_Iraq => new SpecificCultureType("ar-IQ");

        public static SpecificCultureType Arabic_Jordan => new SpecificCultureType("ar-JO");

        public static SpecificCultureType Arabic_Comoros => new SpecificCultureType("ar-KM");

        public static SpecificCultureType Arabic_Kuwait => new SpecificCultureType("ar-KW");

        public static SpecificCultureType Arabic_Lebanon => new SpecificCultureType("ar-LB");

        public static SpecificCultureType Arabic_Libya => new SpecificCultureType("ar-LY");

        public static SpecificCultureType Arabic_Morocco => new SpecificCultureType("ar-MA");

        public static SpecificCultureType Arabic_Mauritania => new SpecificCultureType("ar-MR");

        public static SpecificCultureType Mapudungun => new SpecificCultureType("arn");

        public static SpecificCultureType Mapudungun_Chile => new SpecificCultureType("arn-CL");

        public static SpecificCultureType Arabic_Oman => new SpecificCultureType("ar-OM");

        public static SpecificCultureType Arabic_Palestinian_Authority => new SpecificCultureType("ar-PS");

        public static SpecificCultureType Arabic_Qatar => new SpecificCultureType("ar-QA");

        public static SpecificCultureType Arabic_Saudi_Arabia => new SpecificCultureType("ar-SA");

        public static SpecificCultureType Arabic_Sudan => new SpecificCultureType("ar-SD");

        public static SpecificCultureType Arabic_Somalia => new SpecificCultureType("ar-SO");

        public static SpecificCultureType Arabic_South_Sudan => new SpecificCultureType("ar-SS");

        public static SpecificCultureType Arabic_Syria => new SpecificCultureType("ar-SY");

        public static SpecificCultureType Arabic_Chad => new SpecificCultureType("ar-TD");

        public static SpecificCultureType Arabic_Tunisia => new SpecificCultureType("ar-TN");

        public static SpecificCultureType Arabic_Yemen => new SpecificCultureType("ar-YE");

        public static SpecificCultureType Assamese => new SpecificCultureType("as");

        public static SpecificCultureType Asu => new SpecificCultureType("asa");

        public static SpecificCultureType Asu_Tanzania => new SpecificCultureType("asa-TZ");

        public static SpecificCultureType Assamese_India => new SpecificCultureType("as-IN");

        public static SpecificCultureType Asturian => new SpecificCultureType("ast");

        public static SpecificCultureType Asturian_Spain => new SpecificCultureType("ast-ES");

        public static SpecificCultureType Azerbaijani => new SpecificCultureType("az");

        public static SpecificCultureType Azerbaijani_Cyrillic => new SpecificCultureType("az-Cyrl");

        public static SpecificCultureType Azerbaijani_Cyrillic_Azerbaijan => new SpecificCultureType("az-Cyrl-AZ");

        public static SpecificCultureType Azerbaijani_Latin => new SpecificCultureType("az-Latn");

        public static SpecificCultureType Azerbaijani_Latin_Azerbaijan => new SpecificCultureType("az-Latn-AZ");

        public static SpecificCultureType Bashkir => new SpecificCultureType("ba");

        public static SpecificCultureType Bashkir_Russia => new SpecificCultureType("ba-RU");

        public static SpecificCultureType Basaa => new SpecificCultureType("bas");

        public static SpecificCultureType Basaa_Cameroon => new SpecificCultureType("bas-CM");

        public static SpecificCultureType Belarusian => new SpecificCultureType("be");

        public static SpecificCultureType Belarusian_Belarus => new SpecificCultureType("be-BY");

        public static SpecificCultureType Bemba => new SpecificCultureType("bem");

        public static SpecificCultureType Bemba_Zambia => new SpecificCultureType("bem-ZM");

        public static SpecificCultureType Bena => new SpecificCultureType("bez");

        public static SpecificCultureType Bena_Tanzania => new SpecificCultureType("bez-TZ");

        public static SpecificCultureType Bulgarian => new SpecificCultureType("bg");

        public static SpecificCultureType Bulgarian_Bulgaria => new SpecificCultureType("bg-BG");

        public static SpecificCultureType Edo => new SpecificCultureType("bin");

        public static SpecificCultureType Edo_Nigeria => new SpecificCultureType("bin-NG");

        public static SpecificCultureType Bamanankan => new SpecificCultureType("bm");

        public static SpecificCultureType Bamanankan_Latin => new SpecificCultureType("bm-Latn");

        public static SpecificCultureType Bamanankan_Latin_Mali => new SpecificCultureType("bm-Latn-ML");

        public static SpecificCultureType Bangla => new SpecificCultureType("bn");

        public static SpecificCultureType Bangla_Bangladesh => new SpecificCultureType("bn-BD");

        public static SpecificCultureType Bangla_India => new SpecificCultureType("bn-IN");

        public static SpecificCultureType Tibetan => new SpecificCultureType("bo");

        public static SpecificCultureType Tibetan_China => new SpecificCultureType("bo-CN");

        public static SpecificCultureType Tibetan_India => new SpecificCultureType("bo-IN");

        public static SpecificCultureType Breton => new SpecificCultureType("br");

        public static SpecificCultureType Breton_France => new SpecificCultureType("br-FR");

        public static SpecificCultureType Bodo => new SpecificCultureType("brx");

        public static SpecificCultureType Bodo_India => new SpecificCultureType("brx-IN");

        public static SpecificCultureType Bosnian => new SpecificCultureType("bs");

        public static SpecificCultureType Bosnian_Cyrillic => new SpecificCultureType("bs-Cyrl");

        public static SpecificCultureType Bosnian_Cyrillic_Bosnia_and_Herzegovina =>
            new SpecificCultureType("bs-Cyrl-BA");

        public static SpecificCultureType Bosnian_Latin => new SpecificCultureType("bs-Latn");

        public static SpecificCultureType Bosnian_Latin_Bosnia_and_Herzegovina => new SpecificCultureType("bs-Latn-BA");

        public static SpecificCultureType Blin => new SpecificCultureType("byn");

        public static SpecificCultureType Blin_Eritrea => new SpecificCultureType("byn-ER");

        public static SpecificCultureType Catalan => new SpecificCultureType("ca");

        public static SpecificCultureType Catalan_Andorra => new SpecificCultureType("ca-AD");

        public static SpecificCultureType Catalan_Catalan => new SpecificCultureType("ca-ES");

        public static SpecificCultureType Catalan_France => new SpecificCultureType("ca-FR");

        public static SpecificCultureType Catalan_Italy => new SpecificCultureType("ca-IT");

        public static SpecificCultureType Chechen => new SpecificCultureType("ce");

        public static SpecificCultureType Chechen_Russia => new SpecificCultureType("ce-RU");

        public static SpecificCultureType Chiga => new SpecificCultureType("cgg");

        public static SpecificCultureType Chiga_Uganda => new SpecificCultureType("cgg-UG");

        public static SpecificCultureType Cherokee => new SpecificCultureType("chr");

        public static SpecificCultureType Cherokee_Cherokee => new SpecificCultureType("chr-Cher");

        public static SpecificCultureType Cherokee_Cherokee_Us => new SpecificCultureType("chr-Cher-US");

        public static SpecificCultureType Corsican => new SpecificCultureType("co");

        public static SpecificCultureType Corsican_France => new SpecificCultureType("co-FR");

        public static SpecificCultureType Czech => new SpecificCultureType("cs");

        public static SpecificCultureType Czech_Czech_Republic => new SpecificCultureType("cs-CZ");

        public static SpecificCultureType Church_Slavic => new SpecificCultureType("cu");

        public static SpecificCultureType Church_Slavic_Russia => new SpecificCultureType("cu-RU");

        public static SpecificCultureType Welsh => new SpecificCultureType("cy");

        public static SpecificCultureType Welsh_United_Kingdom => new SpecificCultureType("cy-GB");

        public static SpecificCultureType Danish => new SpecificCultureType("da");

        public static SpecificCultureType Danish_Denmark => new SpecificCultureType("da-DK");

        public static SpecificCultureType Danish_Greenland => new SpecificCultureType("da-GL");

        public static SpecificCultureType Taita => new SpecificCultureType("dav");

        public static SpecificCultureType Taita_Kenya => new SpecificCultureType("dav-KE");

        public static SpecificCultureType German => new SpecificCultureType("de");

        public static SpecificCultureType German_Austria => new SpecificCultureType("de-AT");

        public static SpecificCultureType German_Belgium => new SpecificCultureType("de-BE");

        public static SpecificCultureType German_Switzerland => new SpecificCultureType("de-CH");

        public static SpecificCultureType German_Germany => new SpecificCultureType("de-DE");

        public static SpecificCultureType German_Liechtenstein => new SpecificCultureType("de-LI");

        public static SpecificCultureType German_Luxembourg => new SpecificCultureType("de-LU");

        public static SpecificCultureType Zarma => new SpecificCultureType("dje");

        public static SpecificCultureType Zarma_Niger => new SpecificCultureType("dje-NE");

        public static SpecificCultureType Lower_Sorbian => new SpecificCultureType("dsb");

        public static SpecificCultureType Lower_Sorbian_Germany => new SpecificCultureType("dsb-DE");

        public static SpecificCultureType Duala => new SpecificCultureType("dua");

        public static SpecificCultureType Duala_Cameroon => new SpecificCultureType("dua-CM");

        public static SpecificCultureType Divehi => new SpecificCultureType("dv");

        public static SpecificCultureType Divehi_Maldives => new SpecificCultureType("dv-MV");

        public static SpecificCultureType Jola_Fonyi => new SpecificCultureType("dyo");

        public static SpecificCultureType Jola_Fonyi_Senegal => new SpecificCultureType("dyo-SN");

        public static SpecificCultureType Dzongkha => new SpecificCultureType("dz");

        public static SpecificCultureType Dzongkha_Bhutan => new SpecificCultureType("dz-BT");

        public static SpecificCultureType Embu => new SpecificCultureType("ebu");

        public static SpecificCultureType Embu_Kenya => new SpecificCultureType("ebu-KE");

        public static SpecificCultureType Ewe => new SpecificCultureType("ee");

        public static SpecificCultureType Ewe_Ghana => new SpecificCultureType("ee-GH");

        public static SpecificCultureType Ewe_Togo => new SpecificCultureType("ee-TG");

        public static SpecificCultureType Greek => new SpecificCultureType("el");

        public static SpecificCultureType Greek_Cyprus => new SpecificCultureType("el-CY");

        public static SpecificCultureType Greek_Greece => new SpecificCultureType("el-GR");

        public static SpecificCultureType English => new SpecificCultureType("en");

        public static SpecificCultureType English_World => new SpecificCultureType("en-001");

        public static SpecificCultureType English_Caribbean => new SpecificCultureType("en-029");

        public static SpecificCultureType English_Europe => new SpecificCultureType("en-150");

        public static SpecificCultureType English_Antigua_and_Barbuda => new SpecificCultureType("en-AG");

        public static SpecificCultureType English_Anguilla => new SpecificCultureType("en-AI");

        public static SpecificCultureType English_American_Samoa => new SpecificCultureType("en-AS");

        public static SpecificCultureType English_Austria => new SpecificCultureType("en-AT");

        public static SpecificCultureType English_Australia => new SpecificCultureType("en-AU");

        public static SpecificCultureType English_Barbados => new SpecificCultureType("en-BB");

        public static SpecificCultureType English_Belgium => new SpecificCultureType("en-BE");

        public static SpecificCultureType English_Burundi => new SpecificCultureType("en-BI");

        public static SpecificCultureType English_Bermuda => new SpecificCultureType("en-BM");

        public static SpecificCultureType English_Bahamas => new SpecificCultureType("en-BS");

        public static SpecificCultureType English_Botswana => new SpecificCultureType("en-BW");

        public static SpecificCultureType English_Belize => new SpecificCultureType("en-BZ");

        public static SpecificCultureType English_Canada => new SpecificCultureType("en-CA");

        public static SpecificCultureType English_Cocos_Keeling_Islands => new SpecificCultureType("en-CC");

        public static SpecificCultureType English_Switzerland => new SpecificCultureType("en-CH");

        public static SpecificCultureType English_Cook_Islands => new SpecificCultureType("en-CK");

        public static SpecificCultureType English_Cameroon => new SpecificCultureType("en-CM");

        public static SpecificCultureType English_Christmas_Island => new SpecificCultureType("en-CX");

        public static SpecificCultureType English_Cyprus => new SpecificCultureType("en-CY");

        public static SpecificCultureType English_Germany => new SpecificCultureType("en-DE");

        public static SpecificCultureType English_Denmark => new SpecificCultureType("en-DK");

        public static SpecificCultureType English_Dominica => new SpecificCultureType("en-DM");

        public static SpecificCultureType English_Eritrea => new SpecificCultureType("en-ER");

        public static SpecificCultureType English_Finland => new SpecificCultureType("en-FI");

        public static SpecificCultureType English_Fiji => new SpecificCultureType("en-FJ");

        public static SpecificCultureType English_Falkland_Islands => new SpecificCultureType("en-FK");

        public static SpecificCultureType English_Micronesia => new SpecificCultureType("en-FM");

        public static SpecificCultureType English_United_Kingdom => new SpecificCultureType("en-GB");

        public static SpecificCultureType English_Grenada => new SpecificCultureType("en-GD");

        public static SpecificCultureType English_Guernsey => new SpecificCultureType("en-GG");

        public static SpecificCultureType English_Ghana => new SpecificCultureType("en-GH");

        public static SpecificCultureType English_Gibraltar => new SpecificCultureType("en-GI");

        public static SpecificCultureType English_Gambia => new SpecificCultureType("en-GM");

        public static SpecificCultureType English_Guam => new SpecificCultureType("en-GU");

        public static SpecificCultureType English_Guyana => new SpecificCultureType("en-GY");

        public static SpecificCultureType English_Hong_Kong_SAR => new SpecificCultureType("en-HK");

        public static SpecificCultureType English_Indonesia => new SpecificCultureType("en-ID");

        public static SpecificCultureType English_Ireland => new SpecificCultureType("en-IE");

        public static SpecificCultureType English_Israel => new SpecificCultureType("en-IL");

        public static SpecificCultureType English_Isle_of_Man => new SpecificCultureType("en-IM");

        public static SpecificCultureType English_India => new SpecificCultureType("en-IN");

        public static SpecificCultureType English_British_Indian_Ocean_Territory => new SpecificCultureType("en-IO");

        public static SpecificCultureType English_Jersey => new SpecificCultureType("en-JE");

        public static SpecificCultureType English_Jamaica => new SpecificCultureType("en-JM");

        public static SpecificCultureType English_Kenya => new SpecificCultureType("en-KE");

        public static SpecificCultureType English_Kiribati => new SpecificCultureType("en-KI");

        public static SpecificCultureType English_Saint_Kitts_and_Nevis => new SpecificCultureType("en-KN");

        public static SpecificCultureType English_Cayman_Islands => new SpecificCultureType("en-KY");

        public static SpecificCultureType English_Saint_Lucia => new SpecificCultureType("en-LC");

        public static SpecificCultureType English_Liberia => new SpecificCultureType("en-LR");

        public static SpecificCultureType English_Lesotho => new SpecificCultureType("en-LS");

        public static SpecificCultureType English_Madagascar => new SpecificCultureType("en-MG");

        public static SpecificCultureType English_Marshall_Islands => new SpecificCultureType("en-MH");

        public static SpecificCultureType English_Macao_SAR => new SpecificCultureType("en-MO");

        public static SpecificCultureType English_Northern_Mariana_Islands => new SpecificCultureType("en-MP");

        public static SpecificCultureType English_Montserrat => new SpecificCultureType("en-MS");

        public static SpecificCultureType English_Malta => new SpecificCultureType("en-MT");

        public static SpecificCultureType English_Mauritius => new SpecificCultureType("en-MU");

        public static SpecificCultureType English_Malawi => new SpecificCultureType("en-MW");

        public static SpecificCultureType English_Malaysia => new SpecificCultureType("en-MY");

        public static SpecificCultureType English_Namibia => new SpecificCultureType("en-NA");

        public static SpecificCultureType English_Norfolk_Island => new SpecificCultureType("en-NF");

        public static SpecificCultureType English_Nigeria => new SpecificCultureType("en-NG");

        public static SpecificCultureType English_Netherlands => new SpecificCultureType("en-NL");

        public static SpecificCultureType English_Nauru => new SpecificCultureType("en-NR");

        public static SpecificCultureType English_Niue => new SpecificCultureType("en-NU");

        public static SpecificCultureType English_New_Zealand => new SpecificCultureType("en-NZ");

        public static SpecificCultureType English_Papua_New_Guinea => new SpecificCultureType("en-PG");

        public static SpecificCultureType English_Philippines => new SpecificCultureType("en-PH");

        public static SpecificCultureType English_Pakistan => new SpecificCultureType("en-PK");

        public static SpecificCultureType English_Pitcairn_Islands => new SpecificCultureType("en-PN");

        public static SpecificCultureType English_Puerto_Rico => new SpecificCultureType("en-PR");

        public static SpecificCultureType English_Palau => new SpecificCultureType("en-PW");

        public static SpecificCultureType English_Rwanda => new SpecificCultureType("en-RW");

        public static SpecificCultureType English_Solomon_Islands => new SpecificCultureType("en-SB");

        public static SpecificCultureType English_Seychelles => new SpecificCultureType("en-SC");

        public static SpecificCultureType English_Sudan => new SpecificCultureType("en-SD");

        public static SpecificCultureType English_Sweden => new SpecificCultureType("en-SE");

        public static SpecificCultureType English_Singapore => new SpecificCultureType("en-SG");

        public static SpecificCultureType English_St_Helena_Ascension_Tristan_da_Cunha =>
            new SpecificCultureType("en-SH");

        public static SpecificCultureType English_Slovenia => new SpecificCultureType("en-SI");

        public static SpecificCultureType English_Sierra_Leone => new SpecificCultureType("en-SL");

        public static SpecificCultureType English_South_Sudan => new SpecificCultureType("en-SS");

        public static SpecificCultureType English_Sint_Maarten => new SpecificCultureType("en-SX");

        public static SpecificCultureType English_Swaziland => new SpecificCultureType("en-SZ");

        public static SpecificCultureType English_Turks_and_Caicos_Islands => new SpecificCultureType("en-TC");

        public static SpecificCultureType English_Tokelau => new SpecificCultureType("en-TK");

        public static SpecificCultureType English_Tonga => new SpecificCultureType("en-TO");

        public static SpecificCultureType English_Trinidad_and_Tobago => new SpecificCultureType("en-TT");

        public static SpecificCultureType English_Tuvalu => new SpecificCultureType("en-TV");

        public static SpecificCultureType English_Tanzania => new SpecificCultureType("en-TZ");

        public static SpecificCultureType English_Uganda => new SpecificCultureType("en-UG");

        public static SpecificCultureType English_US_Outlying_Islands => new SpecificCultureType("en-UM");

        public static SpecificCultureType English_United_States => new SpecificCultureType("en-US");

        public static SpecificCultureType English_Saint_Vincent_and_the_Grenadines => new SpecificCultureType("en-VC");

        public static SpecificCultureType English_British_Virgin_Islands => new SpecificCultureType("en-VG");

        public static SpecificCultureType English_US_Virgin_Islands => new SpecificCultureType("en-VI");

        public static SpecificCultureType English_Vanuatu => new SpecificCultureType("en-VU");

        public static SpecificCultureType English_Samoa => new SpecificCultureType("en-WS");

        public static SpecificCultureType English_South_Africa => new SpecificCultureType("en-ZA");

        public static SpecificCultureType English_Zambia => new SpecificCultureType("en-ZM");

        public static SpecificCultureType English_Zimbabwe => new SpecificCultureType("en-ZW");

        public static SpecificCultureType Esperanto => new SpecificCultureType("eo");

        public static SpecificCultureType Esperanto_World => new SpecificCultureType("eo-001");

        public static SpecificCultureType Spanish => new SpecificCultureType("es");

        public static SpecificCultureType Spanish_Latin_America => new SpecificCultureType("es-419");

        public static SpecificCultureType Spanish_Argentina => new SpecificCultureType("es-AR");

        public static SpecificCultureType Spanish_Bolivia => new SpecificCultureType("es-BO");

        public static SpecificCultureType Spanish_Chile => new SpecificCultureType("es-CL");

        public static SpecificCultureType Spanish_Colombia => new SpecificCultureType("es-CO");

        public static SpecificCultureType Spanish_Costa_Rica => new SpecificCultureType("es-CR");

        public static SpecificCultureType Spanish_Cuba => new SpecificCultureType("es-CU");

        public static SpecificCultureType Spanish_Dominican_Republic => new SpecificCultureType("es-DO");

        public static SpecificCultureType Spanish_Ecuador => new SpecificCultureType("es-EC");

        public static SpecificCultureType Spanish_Spain_International_Sort => new SpecificCultureType("es-ES");

        public static SpecificCultureType Spanish_Equatorial_Guinea => new SpecificCultureType("es-GQ");

        public static SpecificCultureType Spanish_Guatemala => new SpecificCultureType("es-GT");

        public static SpecificCultureType Spanish_Honduras => new SpecificCultureType("es-HN");

        public static SpecificCultureType Spanish_Mexico => new SpecificCultureType("es-MX");

        public static SpecificCultureType Spanish_Nicaragua => new SpecificCultureType("es-NI");

        public static SpecificCultureType Spanish_Panama => new SpecificCultureType("es-PA");

        public static SpecificCultureType Spanish_Peru => new SpecificCultureType("es-PE");

        public static SpecificCultureType Spanish_Philippines => new SpecificCultureType("es-PH");

        public static SpecificCultureType Spanish_Puerto_Rico => new SpecificCultureType("es-PR");

        public static SpecificCultureType Spanish_Paraguay => new SpecificCultureType("es-PY");

        public static SpecificCultureType Spanish_El_Salvador => new SpecificCultureType("es-SV");

        public static SpecificCultureType Spanish_United_States => new SpecificCultureType("es-US");

        public static SpecificCultureType Spanish_Uruguay => new SpecificCultureType("es-UY");

        public static SpecificCultureType Spanish_Venezuela => new SpecificCultureType("es-VE");

        public static SpecificCultureType Estonian => new SpecificCultureType("et");

        public static SpecificCultureType Estonian_Estonia => new SpecificCultureType("et-EE");

        public static SpecificCultureType Basque => new SpecificCultureType("eu");

        public static SpecificCultureType Basque_Basque => new SpecificCultureType("eu-ES");

        public static SpecificCultureType Ewondo => new SpecificCultureType("ewo");

        public static SpecificCultureType Ewondo_Cameroon => new SpecificCultureType("ewo-CM");

        public static SpecificCultureType Persian => new SpecificCultureType("fa");

        public static SpecificCultureType Persian_Iran => new SpecificCultureType("fa-IR");

        public static SpecificCultureType Fulah => new SpecificCultureType("ff");

        public static SpecificCultureType Fulah_Cameroon => new SpecificCultureType("ff-CM");

        public static SpecificCultureType Fulah_Guinea => new SpecificCultureType("ff-GN");

        public static SpecificCultureType Fulah_Latn => new SpecificCultureType("ff-Latn");

        public static SpecificCultureType Fulah_Latin_Senegal => new SpecificCultureType("ff-Latn-SN");

        public static SpecificCultureType Fulah_Mauritania => new SpecificCultureType("ff-MR");

        public static SpecificCultureType Fulah_Nigeria => new SpecificCultureType("ff-NG");

        public static SpecificCultureType Finnish => new SpecificCultureType("fi");

        public static SpecificCultureType Finnish_Finland => new SpecificCultureType("fi-FI");

        public static SpecificCultureType Filipino => new SpecificCultureType("fil");

        public static SpecificCultureType Filipino_Philippines => new SpecificCultureType("fil-PH");

        public static SpecificCultureType Faroese => new SpecificCultureType("fo");

        public static SpecificCultureType Faroese_Denmark => new SpecificCultureType("fo-DK");

        public static SpecificCultureType Faroese_Faroe_Islands => new SpecificCultureType("fo-FO");

        public static SpecificCultureType French => new SpecificCultureType("fr");

        public static SpecificCultureType French_Caribbean => new SpecificCultureType("fr-029");

        public static SpecificCultureType French_Belgium => new SpecificCultureType("fr-BE");

        public static SpecificCultureType French_Burkina_Faso => new SpecificCultureType("fr-BF");

        public static SpecificCultureType French_Burundi => new SpecificCultureType("fr-BI");

        public static SpecificCultureType French_Benin => new SpecificCultureType("fr-BJ");

        public static SpecificCultureType French_Saint_Barthélemy => new SpecificCultureType("fr-BL");

        public static SpecificCultureType French_Canada => new SpecificCultureType("fr-CA");

        public static SpecificCultureType French_Congo_DRC => new SpecificCultureType("fr-CD");

        public static SpecificCultureType French_Central_African_Republic => new SpecificCultureType("fr-CF");

        public static SpecificCultureType French_Congo => new SpecificCultureType("fr-CG");

        public static SpecificCultureType French_Switzerland => new SpecificCultureType("fr-CH");

        public static SpecificCultureType French_Cote_dIvoire => new SpecificCultureType("fr-CI");

        public static SpecificCultureType French_Cameroon => new SpecificCultureType("fr-CM");

        public static SpecificCultureType French_Djibouti => new SpecificCultureType("fr-DJ");

        public static SpecificCultureType French_Algeria => new SpecificCultureType("fr-DZ");

        public static SpecificCultureType French_France => new SpecificCultureType("fr-FR");

        public static SpecificCultureType French_Gabon => new SpecificCultureType("fr-GA");

        public static SpecificCultureType French_French_Guiana => new SpecificCultureType("fr-GF");

        public static SpecificCultureType French_Guinea => new SpecificCultureType("fr-GN");

        public static SpecificCultureType French_Guadeloupe => new SpecificCultureType("fr-GP");

        public static SpecificCultureType French_Equatorial_Guinea => new SpecificCultureType("fr-GQ");

        public static SpecificCultureType French_Haiti => new SpecificCultureType("fr-HT");

        public static SpecificCultureType French_Comoros => new SpecificCultureType("fr-KM");

        public static SpecificCultureType French_Luxembourg => new SpecificCultureType("fr-LU");

        public static SpecificCultureType French_Morocco => new SpecificCultureType("fr-MA");

        public static SpecificCultureType French_Monaco => new SpecificCultureType("fr-MC");

        public static SpecificCultureType French_Saint_Martin => new SpecificCultureType("fr-MF");

        public static SpecificCultureType French_Madagascar => new SpecificCultureType("fr-MG");

        public static SpecificCultureType French_Mali => new SpecificCultureType("fr-ML");

        public static SpecificCultureType French_Martinique => new SpecificCultureType("fr-MQ");

        public static SpecificCultureType French_Mauritania => new SpecificCultureType("fr-MR");

        public static SpecificCultureType French_Mauritius => new SpecificCultureType("fr-MU");

        public static SpecificCultureType French_New_Caledonia => new SpecificCultureType("fr-NC");

        public static SpecificCultureType French_Niger => new SpecificCultureType("fr-NE");

        public static SpecificCultureType French_French_Polynesia => new SpecificCultureType("fr-PF");

        public static SpecificCultureType French_Saint_Pierre_and_Miquelon => new SpecificCultureType("fr-PM");

        public static SpecificCultureType French_Réunion => new SpecificCultureType("fr-RE");

        public static SpecificCultureType French_Rwanda => new SpecificCultureType("fr-RW");

        public static SpecificCultureType French_Seychelles => new SpecificCultureType("fr-SC");

        public static SpecificCultureType French_Senegal => new SpecificCultureType("fr-SN");

        public static SpecificCultureType French_Syria => new SpecificCultureType("fr-SY");

        public static SpecificCultureType French_Chad => new SpecificCultureType("fr-TD");

        public static SpecificCultureType French_Togo => new SpecificCultureType("fr-TG");

        public static SpecificCultureType French_Tunisia => new SpecificCultureType("fr-TN");

        public static SpecificCultureType French_Vanuatu => new SpecificCultureType("fr-VU");

        public static SpecificCultureType French_Wallis_and_Futuna => new SpecificCultureType("fr-WF");

        public static SpecificCultureType French_Mayotte => new SpecificCultureType("fr-YT");

        public static SpecificCultureType Friulian => new SpecificCultureType("fur");

        public static SpecificCultureType Friulian_Italy => new SpecificCultureType("fur-IT");

        public static SpecificCultureType Western_Frisian => new SpecificCultureType("fy");

        public static SpecificCultureType Western_Frisian_Netherlands => new SpecificCultureType("fy-NL");

        public static SpecificCultureType Irish => new SpecificCultureType("ga");

        public static SpecificCultureType Irish_Ireland => new SpecificCultureType("ga-IE");

        public static SpecificCultureType Scottish_Gaelic => new SpecificCultureType("gd");

        public static SpecificCultureType Scottish_Gaelic_United_Kingdom => new SpecificCultureType("gd-GB");

        public static SpecificCultureType Galician => new SpecificCultureType("gl");

        public static SpecificCultureType Galician_Galician => new SpecificCultureType("gl-ES");

        public static SpecificCultureType Guarani => new SpecificCultureType("gn");

        public static SpecificCultureType Guarani_Paraguay => new SpecificCultureType("gn-PY");

        public static SpecificCultureType Alsatian => new SpecificCultureType("gsw");

        public static SpecificCultureType Alsatian_Switzerland => new SpecificCultureType("gsw-CH");

        public static SpecificCultureType Alsatian_France => new SpecificCultureType("gsw-FR");

        public static SpecificCultureType Alsatian_Liechtenstein => new SpecificCultureType("gsw-LI");

        public static SpecificCultureType Gujarati => new SpecificCultureType("gu");

        public static SpecificCultureType Gujarati_India => new SpecificCultureType("gu-IN");

        public static SpecificCultureType Gusii => new SpecificCultureType("guz");

        public static SpecificCultureType Gusii_Kenya => new SpecificCultureType("guz-KE");

        public static SpecificCultureType Manx => new SpecificCultureType("gv");

        public static SpecificCultureType Manx_Isle_of_Man => new SpecificCultureType("gv-IM");

        public static SpecificCultureType Hausa => new SpecificCultureType("ha");

        public static SpecificCultureType Hausa_Latin => new SpecificCultureType("ha-Latn");

        public static SpecificCultureType Hausa_Latin_Ghana => new SpecificCultureType("ha-Latn-GH");

        public static SpecificCultureType Hausa_Latin_Niger => new SpecificCultureType("ha-Latn-NE");

        public static SpecificCultureType Hausa_Latin_Nigeria => new SpecificCultureType("ha-Latn-NG");

        public static SpecificCultureType Hawaiian => new SpecificCultureType("haw");

        public static SpecificCultureType Hawaiian_United_States => new SpecificCultureType("haw-US");

        public static SpecificCultureType Hebrew => new SpecificCultureType("he");

        public static SpecificCultureType Hebrew_Israel => new SpecificCultureType("he-IL");

        public static SpecificCultureType Hindi => new SpecificCultureType("hi");

        public static SpecificCultureType Hindi_India => new SpecificCultureType("hi-IN");

        public static SpecificCultureType Croatian => new SpecificCultureType("hr");

        public static SpecificCultureType Croatian_Bosnia_and_Herzegovina => new SpecificCultureType("hr-BA");

        public static SpecificCultureType Croatian_Croatia => new SpecificCultureType("hr-HR");

        public static SpecificCultureType Upper_Sorbian => new SpecificCultureType("hsb");

        public static SpecificCultureType Upper_Sorbian_Germany => new SpecificCultureType("hsb-DE");

        public static SpecificCultureType Hungarian => new SpecificCultureType("hu");

        public static SpecificCultureType Hungarian_Hungary => new SpecificCultureType("hu-HU");

        public static SpecificCultureType Armenian => new SpecificCultureType("hy");

        public static SpecificCultureType Armenian_Armenia => new SpecificCultureType("hy-AM");

        public static SpecificCultureType Interlingua => new SpecificCultureType("ia");

        public static SpecificCultureType Interlingua_World => new SpecificCultureType("ia-001");

        public static SpecificCultureType Interlingua_France => new SpecificCultureType("ia-FR");

        public static SpecificCultureType Ibibio => new SpecificCultureType("ibb");

        public static SpecificCultureType Ibibio_Nigeria => new SpecificCultureType("ibb-NG");

        public static SpecificCultureType Indonesian => new SpecificCultureType("id");

        public static SpecificCultureType Indonesian_Indonesia => new SpecificCultureType("id-ID");

        public static SpecificCultureType Igbo => new SpecificCultureType("ig");

        public static SpecificCultureType Igbo_Nigeria => new SpecificCultureType("ig-NG");

        public static SpecificCultureType Yi => new SpecificCultureType("ii");

        public static SpecificCultureType Yi_China => new SpecificCultureType("ii-CN");

        public static SpecificCultureType Icelandic => new SpecificCultureType("is");

        public static SpecificCultureType Icelandic_Iceland => new SpecificCultureType("is-IS");

        public static SpecificCultureType Italian => new SpecificCultureType("it");

        public static SpecificCultureType Italian_Switzerland => new SpecificCultureType("it-CH");

        public static SpecificCultureType Italian_Italy => new SpecificCultureType("it-IT");

        public static SpecificCultureType Italian_San_Marino => new SpecificCultureType("it-SM");

        public static SpecificCultureType Inuktitut => new SpecificCultureType("iu");

        public static SpecificCultureType Inuktitut_Syllabics => new SpecificCultureType("iu-Cans");

        public static SpecificCultureType Inuktitut_Syllabics_Canada => new SpecificCultureType("iu-Cans-CA");

        public static SpecificCultureType Inuktitut_Latin => new SpecificCultureType("iu-Latn");

        public static SpecificCultureType Inuktitut_Latin_Canada => new SpecificCultureType("iu-Latn-CA");

        public static SpecificCultureType Japanese => new SpecificCultureType("ja");

        public static SpecificCultureType Japanese_Japan => new SpecificCultureType("ja-JP");

        public static SpecificCultureType Ngomba => new SpecificCultureType("jgo");

        public static SpecificCultureType Ngomba_Cameroon => new SpecificCultureType("jgo-CM");

        public static SpecificCultureType Machame => new SpecificCultureType("jmc");

        public static SpecificCultureType Machame_Tanzania => new SpecificCultureType("jmc-TZ");

        public static SpecificCultureType Javanese => new SpecificCultureType("jv");

        public static SpecificCultureType Javanese_Javanese => new SpecificCultureType("jv-Java");

        public static SpecificCultureType Javanese_Javanese_Indonesia => new SpecificCultureType("jv-Java-ID");

        public static SpecificCultureType Javanese_Latn => new SpecificCultureType("jv-Latn");

        public static SpecificCultureType Javanese_Indonesia => new SpecificCultureType("jv-Latn-ID");

        public static SpecificCultureType Georgian => new SpecificCultureType("ka");

        public static SpecificCultureType Kabyle => new SpecificCultureType("kab");

        public static SpecificCultureType Kabyle_Algeria => new SpecificCultureType("kab-DZ");

        public static SpecificCultureType Georgian_Georgia => new SpecificCultureType("ka-GE");

        public static SpecificCultureType Kamba => new SpecificCultureType("kam");

        public static SpecificCultureType Kamba_Kenya => new SpecificCultureType("kam-KE");

        public static SpecificCultureType Makonde => new SpecificCultureType("kde");

        public static SpecificCultureType Makonde_Tanzania => new SpecificCultureType("kde-TZ");

        public static SpecificCultureType Kabuverdianu => new SpecificCultureType("kea");

        public static SpecificCultureType Kabuverdianu_Cabo_Verde => new SpecificCultureType("kea-CV");

        public static SpecificCultureType Koyra_Chiini => new SpecificCultureType("khq");

        public static SpecificCultureType Koyra_Chiini_Mali => new SpecificCultureType("khq-ML");

        public static SpecificCultureType Kikuyu => new SpecificCultureType("ki");

        public static SpecificCultureType Kikuyu_Kenya => new SpecificCultureType("ki-KE");

        public static SpecificCultureType Kazakh => new SpecificCultureType("kk");

        public static SpecificCultureType Kako => new SpecificCultureType("kkj");

        public static SpecificCultureType Kako_Cameroon => new SpecificCultureType("kkj-CM");

        public static SpecificCultureType Kazakh_Kazakhstan => new SpecificCultureType("kk-KZ");

        public static SpecificCultureType Greenlandic => new SpecificCultureType("kl");

        public static SpecificCultureType Greenlandic_Greenland => new SpecificCultureType("kl-GL");

        public static SpecificCultureType Kalenjin => new SpecificCultureType("kln");

        public static SpecificCultureType Kalenjin_Kenya => new SpecificCultureType("kln-KE");

        public static SpecificCultureType Khmer => new SpecificCultureType("km");

        public static SpecificCultureType Khmer_Cambodia => new SpecificCultureType("km-KH");

        public static SpecificCultureType Kannada => new SpecificCultureType("kn");

        public static SpecificCultureType Kannada_India => new SpecificCultureType("kn-IN");

        public static SpecificCultureType Korean => new SpecificCultureType("ko");

        public static SpecificCultureType Konkani => new SpecificCultureType("kok");

        public static SpecificCultureType Konkani_India => new SpecificCultureType("kok-IN");

        public static SpecificCultureType Korean_North_Korea => new SpecificCultureType("ko-KP");

        public static SpecificCultureType Korean_Korea => new SpecificCultureType("ko-KR");

        public static SpecificCultureType Kanuri => new SpecificCultureType("kr");

        public static SpecificCultureType Kanuri_Nigeria => new SpecificCultureType("kr-NG");

        public static SpecificCultureType Kashmiri => new SpecificCultureType("ks");

        public static SpecificCultureType Kashmiri_Perso_Arabic => new SpecificCultureType("ks-Arab");

        public static SpecificCultureType Kashmiri_Perso_Arabic_IN => new SpecificCultureType("ks-Arab-IN");

        public static SpecificCultureType Shambala => new SpecificCultureType("ksb");

        public static SpecificCultureType Shambala_Tanzania => new SpecificCultureType("ksb-TZ");

        public static SpecificCultureType Kashmiri_Devanagari => new SpecificCultureType("ks-Deva");

        public static SpecificCultureType Kashmiri_Devanagari_India => new SpecificCultureType("ks-Deva-IN");

        public static SpecificCultureType Bafia => new SpecificCultureType("ksf");

        public static SpecificCultureType Bafia_Cameroon => new SpecificCultureType("ksf-CM");

        public static SpecificCultureType Ripuarian => new SpecificCultureType("ksh");

        public static SpecificCultureType Ripuarian_Germany => new SpecificCultureType("ksh-DE");

        public static SpecificCultureType Central_Kurdish => new SpecificCultureType("ku");

        public static SpecificCultureType Central_Kurdish_Arab => new SpecificCultureType("ku-Arab");

        public static SpecificCultureType Central_Kurdish_Iraq => new SpecificCultureType("ku-Arab-IQ");

        public static SpecificCultureType Kurdish_Perso_Arabic_Iran => new SpecificCultureType("ku-Arab-IR");

        public static SpecificCultureType Cornish => new SpecificCultureType("kw");

        public static SpecificCultureType Cornish_United_Kingdom => new SpecificCultureType("kw-GB");

        public static SpecificCultureType Kyrgyz => new SpecificCultureType("ky");

        public static SpecificCultureType Kyrgyz_Kyrgyzstan => new SpecificCultureType("ky-KG");

        public static SpecificCultureType Latin => new SpecificCultureType("la");

        public static SpecificCultureType Latin_World => new SpecificCultureType("la-001");

        public static SpecificCultureType Langi => new SpecificCultureType("lag");

        public static SpecificCultureType Langi_Tanzania => new SpecificCultureType("lag-TZ");

        public static SpecificCultureType Luxembourgish => new SpecificCultureType("lb");

        public static SpecificCultureType Luxembourgish_Luxembourg => new SpecificCultureType("lb-LU");

        public static SpecificCultureType Ganda => new SpecificCultureType("lg");

        public static SpecificCultureType Ganda_Uganda => new SpecificCultureType("lg-UG");

        public static SpecificCultureType Lakota => new SpecificCultureType("lkt");

        public static SpecificCultureType Lakota_Unite_States => new SpecificCultureType("lkt-US");

        public static SpecificCultureType Lingala => new SpecificCultureType("ln");

        public static SpecificCultureType Lingala_Angola => new SpecificCultureType("ln-AO");

        public static SpecificCultureType Lingala_Congo_DRC => new SpecificCultureType("ln-CD");

        public static SpecificCultureType Lingala_Central_African_Republic => new SpecificCultureType("ln-CF");

        public static SpecificCultureType Lingala_Congo => new SpecificCultureType("ln-CG");

        public static SpecificCultureType Lao => new SpecificCultureType("lo");

        public static SpecificCultureType Lao_Laos => new SpecificCultureType("lo-LA");

        public static SpecificCultureType Northern_Luri => new SpecificCultureType("lrc");

        public static SpecificCultureType Northern_Luri_Iraq => new SpecificCultureType("lrc-IQ");

        public static SpecificCultureType Northern_Luri_Iran => new SpecificCultureType("lrc-IR");

        public static SpecificCultureType Lithuanian => new SpecificCultureType("lt");

        public static SpecificCultureType Lithuanian_Lithuania => new SpecificCultureType("lt-LT");

        public static SpecificCultureType Luba_Katanga => new SpecificCultureType("lu");

        public static SpecificCultureType Luba_Katanga_Congo_DRC => new SpecificCultureType("lu-CD");

        public static SpecificCultureType Luo => new SpecificCultureType("luo");

        public static SpecificCultureType Luo_Kenya => new SpecificCultureType("luo-KE");

        public static SpecificCultureType Luyia => new SpecificCultureType("luy");

        public static SpecificCultureType Luyia_Kenya => new SpecificCultureType("luy-KE");

        public static SpecificCultureType Latvian => new SpecificCultureType("lv");

        public static SpecificCultureType Latvian_Latvia => new SpecificCultureType("lv-LV");

        public static SpecificCultureType Masai => new SpecificCultureType("mas");

        public static SpecificCultureType Masai_Kenya => new SpecificCultureType("mas-KE");

        public static SpecificCultureType Masai_Tanzania => new SpecificCultureType("mas-TZ");

        public static SpecificCultureType Meru => new SpecificCultureType("mer");

        public static SpecificCultureType Meru_Kenya => new SpecificCultureType("mer-KE");

        public static SpecificCultureType Morisyen => new SpecificCultureType("mfe");

        public static SpecificCultureType Morisyen_Mauritius => new SpecificCultureType("mfe-MU");

        public static SpecificCultureType Malagasy => new SpecificCultureType("mg");

        public static SpecificCultureType Makhuwa_Meetto => new SpecificCultureType("mgh");

        public static SpecificCultureType Makhuwa_Meetto_Mozambique => new SpecificCultureType("mgh-MZ");

        public static SpecificCultureType Malagasy_Madagascar => new SpecificCultureType("mg-MG");

        public static SpecificCultureType Meta => new SpecificCultureType("mgo");

        public static SpecificCultureType Meta_Cameroon => new SpecificCultureType("mgo-CM");

        public static SpecificCultureType Maori => new SpecificCultureType("mi");

        public static SpecificCultureType Maori_New_Zealand => new SpecificCultureType("mi-NZ");

        public static SpecificCultureType Macedonian => new SpecificCultureType("mk");

        public static SpecificCultureType Macedonian_Macedonia_FYRO => new SpecificCultureType("mk-MK");

        public static SpecificCultureType Malayalam => new SpecificCultureType("ml");

        public static SpecificCultureType Malayalam_India => new SpecificCultureType("ml-IN");

        public static SpecificCultureType Mongolian => new SpecificCultureType("mn");

        public static SpecificCultureType Mongolian_Cyrl => new SpecificCultureType("mn-Cyrl");

        public static SpecificCultureType Manipuri => new SpecificCultureType("mni");

        public static SpecificCultureType Manipuri_India => new SpecificCultureType("mni-IN");

        public static SpecificCultureType Mongolian_Mongolia => new SpecificCultureType("mn-MN");

        public static SpecificCultureType Mongolian_Traditional_Mongolian => new SpecificCultureType("mn-Mong");

        public static SpecificCultureType Mongolian_Traditional_Mongolian_China =>
            new SpecificCultureType("mn-Mong-CN");

        public static SpecificCultureType Mongolian_Traditional_Mongolian_Mongolia =>
            new SpecificCultureType("mn-Mong-MN");

        public static SpecificCultureType Mohawk => new SpecificCultureType("moh");

        public static SpecificCultureType Mohawk_Mohawk => new SpecificCultureType("moh-CA");

        public static SpecificCultureType Marathi => new SpecificCultureType("mr");

        public static SpecificCultureType Marathi_India => new SpecificCultureType("mr-IN");

        public static SpecificCultureType Malay => new SpecificCultureType("ms");

        public static SpecificCultureType Malay_Brunei => new SpecificCultureType("ms-BN");

        public static SpecificCultureType Malay_Malaysia => new SpecificCultureType("ms-MY");

        public static SpecificCultureType Malay_Singapore => new SpecificCultureType("ms-SG");

        public static SpecificCultureType Maltese => new SpecificCultureType("mt");

        public static SpecificCultureType Maltese_Malta => new SpecificCultureType("mt-MT");

        public static SpecificCultureType Mundang => new SpecificCultureType("mua");

        public static SpecificCultureType Mundang_Cameroon => new SpecificCultureType("mua-CM");

        public static SpecificCultureType Burmese => new SpecificCultureType("my");

        public static SpecificCultureType Burmese_Myanmar => new SpecificCultureType("my-MM");

        public static SpecificCultureType Mazanderani => new SpecificCultureType("mzn");

        public static SpecificCultureType Mazanderani_Iran => new SpecificCultureType("mzn-IR");

        public static SpecificCultureType Nama => new SpecificCultureType("naq");

        public static SpecificCultureType Nama_Namibia => new SpecificCultureType("naq-NA");

        public static SpecificCultureType Norwegian_Bokmål => new SpecificCultureType("nb");

        public static SpecificCultureType Norwegian_Bokmål_Norway => new SpecificCultureType("nb-NO");

        public static SpecificCultureType Norwegian_Bokmål_Svalbard_and_Jan_Mayen => new SpecificCultureType("nb-SJ");

        public static SpecificCultureType North_Ndebele => new SpecificCultureType("nd");

        public static SpecificCultureType North_Ndebele_Zimbabwe => new SpecificCultureType("nd-ZW");

        public static SpecificCultureType Nepali => new SpecificCultureType("ne");

        public static SpecificCultureType Nepali_India => new SpecificCultureType("ne-IN");

        public static SpecificCultureType Nepali_Nepal => new SpecificCultureType("ne-NP");

        public static SpecificCultureType Dutch => new SpecificCultureType("nl");

        public static SpecificCultureType Dutch_Aruba => new SpecificCultureType("nl-AW");

        public static SpecificCultureType Dutch_Belgium => new SpecificCultureType("nl-BE");

        public static SpecificCultureType Dutch_Bonaire_Sint_Eustatius_and_Saba => new SpecificCultureType("nl-BQ");

        public static SpecificCultureType Dutch_Curaçao => new SpecificCultureType("nl-CW");

        public static SpecificCultureType Dutch_Netherlands => new SpecificCultureType("nl-NL");

        public static SpecificCultureType Dutch_Suriname => new SpecificCultureType("nl-SR");

        public static SpecificCultureType Dutch_Sint_Maarten => new SpecificCultureType("nl-SX");

        public static SpecificCultureType Kwasio => new SpecificCultureType("nmg");

        public static SpecificCultureType Kwasio_Cameroon => new SpecificCultureType("nmg-CM");

        public static SpecificCultureType Norwegian_Nynorsk => new SpecificCultureType("nn");

        public static SpecificCultureType Ngiemboon => new SpecificCultureType("nnh");

        public static SpecificCultureType Ngiemboon_Cameroon => new SpecificCultureType("nnh-CM");

        public static SpecificCultureType Norwegian_Nynorsk_Norway => new SpecificCultureType("nn-NO");

        public static SpecificCultureType Norwegian => new SpecificCultureType("no");

        public static SpecificCultureType ko => new SpecificCultureType("nqo");

        public static SpecificCultureType ko_Guinea => new SpecificCultureType("nqo-GN");

        public static SpecificCultureType South_Ndebele => new SpecificCultureType("nr");

        public static SpecificCultureType South_Ndebele_South_Africa => new SpecificCultureType("nr-ZA");

        public static SpecificCultureType Sesotho_sa_Leboa => new SpecificCultureType("nso");

        public static SpecificCultureType Sesotho_sa_Leboa_South_Africa => new SpecificCultureType("nso-ZA");

        public static SpecificCultureType Nuer => new SpecificCultureType("nus");

        public static SpecificCultureType Nuer_South_Sudan => new SpecificCultureType("nus-SS");

        public static SpecificCultureType Nyankole => new SpecificCultureType("nyn");

        public static SpecificCultureType Nyankole_Uganda => new SpecificCultureType("nyn-UG");

        public static SpecificCultureType Occitan => new SpecificCultureType("oc");

        public static SpecificCultureType Occitan_France => new SpecificCultureType("oc-FR");

        public static SpecificCultureType Oromo => new SpecificCultureType("om");

        public static SpecificCultureType Oromo_Ethiopia => new SpecificCultureType("om-ET");

        public static SpecificCultureType Oromo_Kenya => new SpecificCultureType("om-KE");

        public static SpecificCultureType Odia => new SpecificCultureType("or");

        public static SpecificCultureType Odia_India => new SpecificCultureType("or-IN");

        public static SpecificCultureType Ossetian => new SpecificCultureType("os");

        public static SpecificCultureType Ossetian_Georgia => new SpecificCultureType("os-GE");

        public static SpecificCultureType Ossetian_Russia => new SpecificCultureType("os-RU");

        public static SpecificCultureType Punjabi => new SpecificCultureType("pa");

        public static SpecificCultureType Punjabi_Arab => new SpecificCultureType("pa-Arab");

        public static SpecificCultureType Punjabi_Pakistan => new SpecificCultureType("pa-Arab-PK");

        public static SpecificCultureType Punjabi_India => new SpecificCultureType("pa-IN");

        public static SpecificCultureType Papiamento => new SpecificCultureType("pap");

        public static SpecificCultureType Papiamento_Caribbean => new SpecificCultureType("pap-029");

        public static SpecificCultureType Polish => new SpecificCultureType("pl");

        public static SpecificCultureType Polish_Poland => new SpecificCultureType("pl-PL");

        public static SpecificCultureType Prussian => new SpecificCultureType("prg");

        public static SpecificCultureType Prussian_World => new SpecificCultureType("prg-001");

        public static SpecificCultureType Dari => new SpecificCultureType("prs");

        public static SpecificCultureType Dari_Afghanistan => new SpecificCultureType("prs-AF");

        public static SpecificCultureType Pashto => new SpecificCultureType("ps");

        public static SpecificCultureType Pashto_Afghanistan => new SpecificCultureType("ps-AF");

        public static SpecificCultureType Portuguese => new SpecificCultureType("pt");

        public static SpecificCultureType Portuguese_Angola => new SpecificCultureType("pt-AO");

        public static SpecificCultureType Portuguese_Brazil => new SpecificCultureType("pt-BR");

        public static SpecificCultureType Portuguese_Cabo_Verde => new SpecificCultureType("pt-CV");

        public static SpecificCultureType Portuguese_Guinea_Bissau => new SpecificCultureType("pt-GW");

        public static SpecificCultureType Portuguese_Macao_SAR => new SpecificCultureType("pt-MO");

        public static SpecificCultureType Portuguese_Mozambique => new SpecificCultureType("pt-MZ");

        public static SpecificCultureType Portuguese_Portugal => new SpecificCultureType("pt-PT");

        public static SpecificCultureType Portuguese_São_Tomé_and_Príncipe => new SpecificCultureType("pt-ST");

        public static SpecificCultureType Portuguese_Timor_Leste => new SpecificCultureType("pt-TL");

        public static SpecificCultureType iche => new SpecificCultureType("quc");

        public static SpecificCultureType iche_Latn => new SpecificCultureType("quc-Latn");

        public static SpecificCultureType iche_Guatemala => new SpecificCultureType("quc-Latn-GT");

        public static SpecificCultureType Quechua => new SpecificCultureType("quz");

        public static SpecificCultureType Quechua_Bolivia => new SpecificCultureType("quz-BO");

        public static SpecificCultureType Quichua_Ecuador => new SpecificCultureType("quz-EC");

        public static SpecificCultureType Quechua_Peru => new SpecificCultureType("quz-PE");

        public static SpecificCultureType Romansh => new SpecificCultureType("rm");

        public static SpecificCultureType Romansh_Switzerland => new SpecificCultureType("rm-CH");

        public static SpecificCultureType Rundi => new SpecificCultureType("rn");

        public static SpecificCultureType Rundi_Burundi => new SpecificCultureType("rn-BI");

        public static SpecificCultureType Romanian => new SpecificCultureType("ro");

        public static SpecificCultureType Rombo => new SpecificCultureType("rof");

        public static SpecificCultureType Rombo_Tanzania => new SpecificCultureType("rof-TZ");

        public static SpecificCultureType Romanian_Moldova => new SpecificCultureType("ro-MD");

        public static SpecificCultureType Romanian_Romania => new SpecificCultureType("ro-RO");

        public static SpecificCultureType Russian => new SpecificCultureType("ru");

        public static SpecificCultureType Russian_Belarus => new SpecificCultureType("ru-BY");

        public static SpecificCultureType Russian_Kyrgyzstan => new SpecificCultureType("ru-KG");

        public static SpecificCultureType Russian_Kazakhstan => new SpecificCultureType("ru-KZ");

        public static SpecificCultureType Russian_Moldova => new SpecificCultureType("ru-MD");

        public static SpecificCultureType Russian_Russia => new SpecificCultureType("ru-RU");

        public static SpecificCultureType Russian_Ukraine => new SpecificCultureType("ru-UA");

        public static SpecificCultureType Kinyarwanda => new SpecificCultureType("rw");

        public static SpecificCultureType Rwa => new SpecificCultureType("rwk");

        public static SpecificCultureType Rwa_Tanzania => new SpecificCultureType("rwk-TZ");

        public static SpecificCultureType Kinyarwanda_Rwanda => new SpecificCultureType("rw-RW");

        public static SpecificCultureType Sanskrit => new SpecificCultureType("sa");

        public static SpecificCultureType Sakha => new SpecificCultureType("sah");

        public static SpecificCultureType Sakha_Russia => new SpecificCultureType("sah-RU");

        public static SpecificCultureType Sanskrit_India => new SpecificCultureType("sa-IN");

        public static SpecificCultureType Samburu => new SpecificCultureType("saq");

        public static SpecificCultureType Samburu_Kenya => new SpecificCultureType("saq-KE");

        public static SpecificCultureType Sangu => new SpecificCultureType("sbp");

        public static SpecificCultureType Sangu_Tanzania => new SpecificCultureType("sbp-TZ");

        public static SpecificCultureType Sindhi => new SpecificCultureType("sd");

        public static SpecificCultureType Sindhi_Arab => new SpecificCultureType("sd-Arab");

        public static SpecificCultureType Sindhi_Pakistan => new SpecificCultureType("sd-Arab-PK");

        public static SpecificCultureType Sindhi_Devanagari => new SpecificCultureType("sd-Deva");

        public static SpecificCultureType Sindhi_Devanagari_India => new SpecificCultureType("sd-Deva-IN");

        public static SpecificCultureType Northern_Sami => new SpecificCultureType("se");

        public static SpecificCultureType Sami_Northern_Finland => new SpecificCultureType("se-FI");

        public static SpecificCultureType Sena => new SpecificCultureType("seh");

        public static SpecificCultureType Sena_Mozambique => new SpecificCultureType("seh-MZ");

        public static SpecificCultureType Sami_Northern_Norway => new SpecificCultureType("se-NO");

        public static SpecificCultureType Koyraboro_Senni => new SpecificCultureType("ses");

        public static SpecificCultureType Sami_Northern_Sweden => new SpecificCultureType("se-SE");

        public static SpecificCultureType Koyraboro_Senni_Mali => new SpecificCultureType("ses-ML");

        public static SpecificCultureType Sango => new SpecificCultureType("sg");

        public static SpecificCultureType Sango_Central_African_Republic => new SpecificCultureType("sg-CF");

        public static SpecificCultureType Tachelhit => new SpecificCultureType("shi");

        public static SpecificCultureType Tachelhit_Latin => new SpecificCultureType("shi-Latn");

        public static SpecificCultureType Tachelhit_Latin_Morocco => new SpecificCultureType("shi-Latn-MA");

        public static SpecificCultureType Tachelhit_Tifinagh => new SpecificCultureType("shi-Tfng");

        public static SpecificCultureType Tachelhit_Tifinagh_Morocco => new SpecificCultureType("shi-Tfng-MA");

        public static SpecificCultureType Sinhala => new SpecificCultureType("si");

        public static SpecificCultureType Sinhala_Sri_Lanka => new SpecificCultureType("si-LK");

        public static SpecificCultureType Slovak => new SpecificCultureType("sk");

        public static SpecificCultureType Slovak_Slovakia => new SpecificCultureType("sk-SK");

        public static SpecificCultureType Slovenian => new SpecificCultureType("sl");

        public static SpecificCultureType Slovenian_Slovenia => new SpecificCultureType("sl-SI");

        public static SpecificCultureType Sami_Southern => new SpecificCultureType("sma");

        public static SpecificCultureType Sami_Southern_Norway => new SpecificCultureType("sma-NO");

        public static SpecificCultureType Sami_Southern_Sweden => new SpecificCultureType("sma-SE");

        public static SpecificCultureType Sami_Lule => new SpecificCultureType("smj");

        public static SpecificCultureType Sami_Lule_Norway => new SpecificCultureType("smj-NO");

        public static SpecificCultureType Sami_Lule_Sweden => new SpecificCultureType("smj-SE");

        public static SpecificCultureType Sami_Inari => new SpecificCultureType("smn");

        public static SpecificCultureType Sami_Inari_Finland => new SpecificCultureType("smn-FI");

        public static SpecificCultureType Sami_Skolt => new SpecificCultureType("sms");

        public static SpecificCultureType Sami_Skolt_Finland => new SpecificCultureType("sms-FI");

        public static SpecificCultureType Shona => new SpecificCultureType("sn");

        public static SpecificCultureType Shona_Latin => new SpecificCultureType("sn-Latn");

        public static SpecificCultureType Shona_Latin_Zimbabwe => new SpecificCultureType("sn-Latn-ZW");

        public static SpecificCultureType Somali => new SpecificCultureType("so");

        public static SpecificCultureType Somali_Djibouti => new SpecificCultureType("so-DJ");

        public static SpecificCultureType Somali_Ethiopia => new SpecificCultureType("so-ET");

        public static SpecificCultureType Somali_Kenya => new SpecificCultureType("so-KE");

        public static SpecificCultureType Somali_Somalia => new SpecificCultureType("so-SO");

        public static SpecificCultureType Albanian => new SpecificCultureType("sq");

        public static SpecificCultureType Albanian_Albania => new SpecificCultureType("sq-AL");

        public static SpecificCultureType Albanian_Macedonia_FYRO => new SpecificCultureType("sq-MK");

        public static SpecificCultureType Albanian_Kosovo => new SpecificCultureType("sq-XK");

        public static SpecificCultureType Serbian => new SpecificCultureType("sr");

        public static SpecificCultureType Serbian_Cyrillic => new SpecificCultureType("sr-Cyrl");

        public static SpecificCultureType Serbian_Cyrillic_Bosnia_and_Herzegovina =>
            new SpecificCultureType("sr-Cyrl-BA");

        public static SpecificCultureType Serbian_Cyrillic_Montenegro => new SpecificCultureType("sr-Cyrl-ME");

        public static SpecificCultureType Serbian_Cyrillic_Serbia => new SpecificCultureType("sr-Cyrl-RS");

        public static SpecificCultureType Serbian_Cyrillic_Kosovo => new SpecificCultureType("sr-Cyrl-XK");

        public static SpecificCultureType Serbian_Latin => new SpecificCultureType("sr-Latn");

        public static SpecificCultureType Serbian_Latin_Bosnia_and_Herzegovina => new SpecificCultureType("sr-Latn-BA");

        public static SpecificCultureType Serbian_Latin_Montenegro => new SpecificCultureType("sr-Latn-ME");

        public static SpecificCultureType Serbian_Latin_Serbia => new SpecificCultureType("sr-Latn-RS");

        public static SpecificCultureType Serbian_Latin_Kosovo => new SpecificCultureType("sr-Latn-XK");

        public static SpecificCultureType siSwati => new SpecificCultureType("ss");

        public static SpecificCultureType siSwati_Swaziland => new SpecificCultureType("ss-SZ");

        public static SpecificCultureType Saho => new SpecificCultureType("ssy");

        public static SpecificCultureType Saho_Eritrea => new SpecificCultureType("ssy-ER");

        public static SpecificCultureType siSwati_South_Africa => new SpecificCultureType("ss-ZA");

        public static SpecificCultureType Sesotho => new SpecificCultureType("st");

        public static SpecificCultureType Sesotho_Lesotho => new SpecificCultureType("st-LS");

        public static SpecificCultureType Sesotho_South_Africa => new SpecificCultureType("st-ZA");

        public static SpecificCultureType Swedish => new SpecificCultureType("sv");

        public static SpecificCultureType Swedish_Åland_Islands => new SpecificCultureType("sv-AX");

        public static SpecificCultureType Swedish_Finland => new SpecificCultureType("sv-FI");

        public static SpecificCultureType Swedish_Sweden => new SpecificCultureType("sv-SE");

        public static SpecificCultureType Kiswahili => new SpecificCultureType("sw");

        public static SpecificCultureType Kiswahili_Congo_DRC => new SpecificCultureType("sw-CD");

        public static SpecificCultureType Kiswahili_Kenya => new SpecificCultureType("sw-KE");

        public static SpecificCultureType Kiswahili_Tanzania => new SpecificCultureType("sw-TZ");

        public static SpecificCultureType Kiswahili_Uganda => new SpecificCultureType("sw-UG");

        public static SpecificCultureType Syriac => new SpecificCultureType("syr");

        public static SpecificCultureType Syriac_Syria => new SpecificCultureType("syr-SY");

        public static SpecificCultureType Tamil => new SpecificCultureType("ta");

        public static SpecificCultureType Tamil_India => new SpecificCultureType("ta-IN");

        public static SpecificCultureType Tamil_Sri_Lanka => new SpecificCultureType("ta-LK");

        public static SpecificCultureType Tamil_Malaysia => new SpecificCultureType("ta-MY");

        public static SpecificCultureType Tamil_Singapore => new SpecificCultureType("ta-SG");

        public static SpecificCultureType Telugu => new SpecificCultureType("te");

        public static SpecificCultureType Telugu_India => new SpecificCultureType("te-IN");

        public static SpecificCultureType Teso => new SpecificCultureType("teo");

        public static SpecificCultureType Teso_Kenya => new SpecificCultureType("teo-KE");

        public static SpecificCultureType Teso_Uganda => new SpecificCultureType("teo-UG");

        public static SpecificCultureType Tajik => new SpecificCultureType("tg");

        public static SpecificCultureType Tajik_Cyrillic => new SpecificCultureType("tg-Cyrl");

        public static SpecificCultureType Tajik_Cyrillic_Tajikistan => new SpecificCultureType("tg-Cyrl-TJ");

        public static SpecificCultureType Thai => new SpecificCultureType("th");

        public static SpecificCultureType Thai_Thailand => new SpecificCultureType("th-TH");

        public static SpecificCultureType Tigrinya => new SpecificCultureType("ti");

        public static SpecificCultureType Tigrinya_Eritrea => new SpecificCultureType("ti-ER");

        public static SpecificCultureType Tigrinya_Ethiopia => new SpecificCultureType("ti-ET");

        public static SpecificCultureType Tigre => new SpecificCultureType("tig");

        public static SpecificCultureType Tigre_Eritrea => new SpecificCultureType("tig-ER");

        public static SpecificCultureType Turkmen => new SpecificCultureType("tk");

        public static SpecificCultureType Turkmen_Turkmenistan => new SpecificCultureType("tk-TM");

        public static SpecificCultureType Setswana => new SpecificCultureType("tn");

        public static SpecificCultureType Setswana_Botswana => new SpecificCultureType("tn-BW");

        public static SpecificCultureType Setswana_South_Africa => new SpecificCultureType("tn-ZA");

        public static SpecificCultureType Tongan => new SpecificCultureType("to");

        public static SpecificCultureType Tongan_Tonga => new SpecificCultureType("to-TO");

        public static SpecificCultureType Turkish => new SpecificCultureType("tr");

        public static SpecificCultureType Turkish_Cyprus => new SpecificCultureType("tr-CY");

        public static SpecificCultureType Turkish_Turkey => new SpecificCultureType("tr-TR");

        public static SpecificCultureType Tsonga => new SpecificCultureType("ts");

        public static SpecificCultureType Xitsonga_South_Africa => new SpecificCultureType("ts-ZA");

        public static SpecificCultureType Tatar => new SpecificCultureType("tt");

        public static SpecificCultureType Tatar_Russia => new SpecificCultureType("tt-RU");

        public static SpecificCultureType Tasawaq => new SpecificCultureType("twq");

        public static SpecificCultureType Tasawaq_Niger => new SpecificCultureType("twq-NE");

        public static SpecificCultureType Central_Atlas_Tamazight => new SpecificCultureType("tzm");

        public static SpecificCultureType Central_Atlas_Tamazight_Arabic => new SpecificCultureType("tzm-Arab");

        public static SpecificCultureType Central_Atlas_Tamazight_Arabic_Morocco =>
            new SpecificCultureType("tzm-Arab-MA");

        public static SpecificCultureType Central_Atlas_Tamazight_Latin => new SpecificCultureType("tzm-Latn");

        public static SpecificCultureType Central_Atlas_Tamazight_Latin_Algeria =>
            new SpecificCultureType("tzm-Latn-DZ");

        public static SpecificCultureType Central_Atlas_Tamazight_Latin_Morocco =>
            new SpecificCultureType("tzm-Latn-MA");

        public static SpecificCultureType Central_Atlas_Tamazight_Tifinagh => new SpecificCultureType("tzm-Tfng");

        public static SpecificCultureType Central_Atlas_Tamazight_Tifinagh_Morocco =>
            new SpecificCultureType("tzm-Tfng-MA");

        public static SpecificCultureType Uyghur => new SpecificCultureType("ug");

        public static SpecificCultureType Uyghur_China => new SpecificCultureType("ug-CN");

        public static SpecificCultureType Ukrainian => new SpecificCultureType("uk");

        public static SpecificCultureType Ukrainian_Ukraine => new SpecificCultureType("uk-UA");

        public static SpecificCultureType Urdu => new SpecificCultureType("ur");

        public static SpecificCultureType Urdu_India => new SpecificCultureType("ur-IN");

        public static SpecificCultureType Urdu_Pakistan => new SpecificCultureType("ur-PK");

        public static SpecificCultureType Uzbek => new SpecificCultureType("uz");

        public static SpecificCultureType Uzbek_Perso_Arabic => new SpecificCultureType("uz-Arab");

        public static SpecificCultureType Uzbek_Perso_Arabic_Afghanistan => new SpecificCultureType("uz-Arab-AF");

        public static SpecificCultureType Uzbek_Cyrillic => new SpecificCultureType("uz-Cyrl");

        public static SpecificCultureType Uzbek_Cyrillic_Uzbekistan => new SpecificCultureType("uz-Cyrl-UZ");

        public static SpecificCultureType Uzbek_Latin => new SpecificCultureType("uz-Latn");

        public static SpecificCultureType Uzbek_Latin_Uzbekistan => new SpecificCultureType("uz-Latn-UZ");

        public static SpecificCultureType Vai => new SpecificCultureType("vai");

        public static SpecificCultureType Vai_Latin => new SpecificCultureType("vai-Latn");

        public static SpecificCultureType Vai_Latin_Liberia => new SpecificCultureType("vai-Latn-LR");

        public static SpecificCultureType Vai_Vai => new SpecificCultureType("vai-Vaii");

        public static SpecificCultureType Vai_Vai_Liberia => new SpecificCultureType("vai-Vaii-LR");

        public static SpecificCultureType Venda => new SpecificCultureType("ve");

        public static SpecificCultureType Venda_South_Africa => new SpecificCultureType("ve-ZA");

        public static SpecificCultureType Vietnamese => new SpecificCultureType("vi");

        public static SpecificCultureType Vietnamese_Vietnam => new SpecificCultureType("vi-VN");

        public static SpecificCultureType Volapük => new SpecificCultureType("vo");

        public static SpecificCultureType Volapük_World => new SpecificCultureType("vo-001");

        public static SpecificCultureType Vunjo => new SpecificCultureType("vun");

        public static SpecificCultureType Vunjo_Tanzania => new SpecificCultureType("vun-TZ");

        public static SpecificCultureType Walser => new SpecificCultureType("wae");

        public static SpecificCultureType Walser_Switzerland => new SpecificCultureType("wae-CH");

        public static SpecificCultureType Wolaytta => new SpecificCultureType("wal");

        public static SpecificCultureType Wolaytta_Ethiopia => new SpecificCultureType("wal-ET");

        public static SpecificCultureType Wolof => new SpecificCultureType("wo");

        public static SpecificCultureType Wolof_Senegal => new SpecificCultureType("wo-SN");

        public static SpecificCultureType isiXhosa => new SpecificCultureType("xh");

        public static SpecificCultureType isiXhosa_South_Africa => new SpecificCultureType("xh-ZA");

        public static SpecificCultureType Soga => new SpecificCultureType("xog");

        public static SpecificCultureType Soga_Uganda => new SpecificCultureType("xog-UG");

        public static SpecificCultureType Yangben => new SpecificCultureType("yav");

        public static SpecificCultureType Yangben_Cameroon => new SpecificCultureType("yav-CM");

        public static SpecificCultureType Yiddish => new SpecificCultureType("yi");

        public static SpecificCultureType Yiddish_World => new SpecificCultureType("yi-001");

        public static SpecificCultureType Yoruba => new SpecificCultureType("yo");

        public static SpecificCultureType Yoruba_Benin => new SpecificCultureType("yo-BJ");

        public static SpecificCultureType Yoruba_Nigeria => new SpecificCultureType("yo-NG");

        public static SpecificCultureType Standard_Moroccan_Tamazight => new SpecificCultureType("zgh");

        public static SpecificCultureType Standard_Moroccan_Tamazight_Tifinagh => new SpecificCultureType("zgh-Tfng");

        public static SpecificCultureType Standard_Moroccan_Tamazight_Tifinagh_Morocco =>
            new SpecificCultureType("zgh-Tfng-MA");

        public static SpecificCultureType Chinese => new SpecificCultureType("zh");

        public static SpecificCultureType Chinese_Simplified_Legacy => new SpecificCultureType("zh-CHS");

        public static SpecificCultureType Chinese_Traditional_Legacy => new SpecificCultureType("zh-CHT");

        public static SpecificCultureType Chinese_Simplified_China => new SpecificCultureType("zh-CN");

        public static SpecificCultureType Chinese_Simplified => new SpecificCultureType("zh-Hans");

        public static SpecificCultureType Chinese_Simplified_Han_Hong_Kong_SAR => new SpecificCultureType("zh-Hans-HK");

        public static SpecificCultureType Chinese_Simplified_Han_Macao_SAR => new SpecificCultureType("zh-Hans-MO");

        public static SpecificCultureType Chinese_Traditional => new SpecificCultureType("zh-Hant");

        public static SpecificCultureType Chinese_Traditional_Hong_Kong_SAR => new SpecificCultureType("zh-HK");

        public static SpecificCultureType Chinese_Traditional_Macao_SAR => new SpecificCultureType("zh-MO");

        public static SpecificCultureType Chinese_Simplified_Singapore => new SpecificCultureType("zh-SG");

        public static SpecificCultureType Chinese_Traditional_Taiwan => new SpecificCultureType("zh-TW");

        public static SpecificCultureType isiZulu => new SpecificCultureType("zu");

        public static SpecificCultureType isiZulu_South_Africa => new SpecificCultureType("zu-ZA");
    }

    public static class CultureHelper
    {
        public static CultureInfo GetDefaultCulture
        {
            get
            {
                return GetValidCultures()
                    .FirstOrDefault(x => x.Name.Equals(SpecificCultureType.English.Key));
            }
        }

        public static string GetCurrentCultureName => Thread.CurrentThread.CurrentCulture.Name;

        public static string GetCurrentCultureDisplayName => Thread.CurrentThread.CurrentCulture.DisplayName;

        public static string GetCurrentNeutralCultureName => Thread.CurrentThread.CurrentUICulture.Name;

        public static string GetCurrentNeutralCultureDisplayName => Thread.CurrentThread.CurrentUICulture.DisplayName;

        /// <summary>
        ///     Returns true if the language is a right-to-left language. Otherwise, false.
        /// </summary>
        public static bool IsRightToLeft()
        {
            return Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft;
        }

        /// <summary>
        ///     Returns a valid culture name based on "name" parameter. If "name" is not valid, it returns the default culture
        ///     "en-US"
        /// </summary>
        public static CultureInfo GetValidCulture(string key)
        {
            if (string.IsNullOrEmpty(key)) return GetDefaultCulture;

            return GetValidCultures().FirstOrDefault(x => x.Name.Equals(key));
        }

        public static string GetSpecificCulture(string key)
        {
            return CultureInfo.CreateSpecificCulture(key).Name;
        }

        public static CultureInfo GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture;
        }

        public static CultureInfo GetCurrentNeutralCulture()
        {
            return Thread.CurrentThread.CurrentUICulture;
        }

        public static void SetCurrentCulture(string key)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(key);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(key);
        }

        public static List<CultureInfo> GetValidCultures()
        {
            var list = new List<CultureInfo>();

            foreach (var ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
                try
                {
                    list.Add(CultureInfo.CreateSpecificCulture(ci.Name));
                }
                catch
                {
                }

            list.OrderBy(x => x.Name);
            return list;
        }
    }
}