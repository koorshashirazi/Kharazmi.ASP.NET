namespace Kharazmi.AspNetMvc.Core.Managers.PdfManager
{
    public sealed class PaperSize
    {
        private readonly Length _height;
        private readonly Length _width;

        public PaperSize(Length width, Length height)
        {
            _width = width;
            _height = height;
        }

        public static PaperSize Letter => new PaperSize(8.5.Inches(), 11.Inches());

        public static PaperSize Legal => new PaperSize(8.5.Inches(), 14.Inches());

        public static PaperSize A4 => new PaperSize(210.Millimeters(), 297.Millimeters());

        public static PaperSize CSheet => new PaperSize(17.Inches(), 22.Inches());

        public static PaperSize DSheet => new PaperSize(22.Inches(), 34.Inches());

        public static PaperSize ESheet => new PaperSize(34.Inches(), 44.Inches());

        public static PaperSize LetterSmall => new PaperSize(8.5.Inches(), 11.Inches());

        public static PaperSize Tabloid => new PaperSize(11.Inches(), 17.Inches());

        public static PaperSize Ledger => new PaperSize(17.Inches(), 11.Inches());

        public static PaperSize Statement => new PaperSize(5.5.Inches(), 8.5.Inches());

        public static PaperSize Executive => new PaperSize(7.25.Inches(), 10.5.Inches());

        public static PaperSize A3 => new PaperSize(297.Millimeters(), 420.Millimeters());

        public static PaperSize A4Small => new PaperSize(210.Millimeters(), 297.Millimeters());

        public static PaperSize A5 => new PaperSize(148.Millimeters(), 210.Millimeters());

        public static PaperSize B4 => new PaperSize(250.Millimeters(), 353.Millimeters());

        public static PaperSize B5 => new PaperSize(176.Millimeters(), 250.Millimeters());

        public static PaperSize Folio => new PaperSize(8.5.Inches(), 13.Inches());

        public static PaperSize Quarto => new PaperSize(215.Millimeters(), 275.Millimeters());

        public static PaperSize Standard10X14 => new PaperSize(10.Inches(), 14.Inches());

        public static PaperSize Standard11X17 => new PaperSize(11.Inches(), 17.Inches());

        public static PaperSize Note => new PaperSize(8.5.Inches(), 11.Inches());

        public static PaperSize Number9Envelope => new PaperSize(3.875.Inches(), 8.875.Inches());

        public static PaperSize Number10Envelope => new PaperSize(4.125.Inches(), 9.5.Inches());

        public static PaperSize Number11Envelope => new PaperSize(4.5.Inches(), 10.375.Inches());

        public static PaperSize Number12Envelope => new PaperSize(4.75.Inches(), 11.Inches());

        public static PaperSize Number14Envelope => new PaperSize(5.Inches(), 11.5.Inches());

        public static PaperSize DlEnvelope => new PaperSize(110.Millimeters(), 220.Millimeters());

        public static PaperSize C5Envelope => new PaperSize(162.Millimeters(), 229.Millimeters());

        public static PaperSize C3Envelope => new PaperSize(324.Millimeters(), 458.Millimeters());

        public static PaperSize C4Envelope => new PaperSize(229.Millimeters(), 324.Millimeters());

        public static PaperSize C6Envelope => new PaperSize(114.Millimeters(), 162.Millimeters());

        public static PaperSize C65Envelope => new PaperSize(114.Millimeters(), 229.Millimeters());

        public static PaperSize B4Envelope => new PaperSize(250.Millimeters(), 353.Millimeters());

        public static PaperSize B5Envelope => new PaperSize(176.Millimeters(), 250.Millimeters());

        public static PaperSize B6Envelope => new PaperSize(176.Millimeters(), 125.Millimeters());

        public static PaperSize ItalyEnvelope => new PaperSize(110.Millimeters(), 230.Millimeters());

        public static PaperSize MonarchEnvelope => new PaperSize(3.875.Inches(), 7.5.Inches());

        public static PaperSize PersonalEnvelope => new PaperSize(3.625.Inches(), 6.5.Inches());

        public static PaperSize UsStandardFanfold => new PaperSize(14.875.Inches(), 11.Inches());

        public static PaperSize GermanStandardFanfold => new PaperSize(8.5.Inches(), 12.Inches());

        public static PaperSize GermanLegalFanfold => new PaperSize(8.5.Inches(), 13.Inches());

        public static PaperSize IsoB4 => new PaperSize(250.Millimeters(), 353.Millimeters());

        public static PaperSize JapanesePostcard => new PaperSize(100.Millimeters(), 148.Millimeters());

        public static PaperSize Standard9X11 => new PaperSize(9.Inches(), 11.Inches());

        public static PaperSize Standard10X11 => new PaperSize(10.Inches(), 11.Inches());

        public static PaperSize Standard15X11 => new PaperSize(15.Inches(), 11.Inches());

        public static PaperSize InviteEnvelope => new PaperSize(220.Millimeters(), 220.Millimeters());

        public static PaperSize LetterExtra => new PaperSize(9.275.Inches(), 12.Inches());

        public static PaperSize LegalExtra => new PaperSize(9.275.Inches(), 15.Inches());

        public static PaperSize TabloidExtra => new PaperSize(11.69.Inches(), 18.Inches());

        public static PaperSize A4Extra => new PaperSize(236.Millimeters(), 322.Millimeters());

        public static PaperSize LetterTransverse => new PaperSize(8.275.Inches(), 11.Inches());

        public static PaperSize A4Transverse => new PaperSize(210.Millimeters(), 297.Millimeters());

        public static PaperSize LetterExtraTransverse => new PaperSize(9.275.Inches(), 12.Inches());

        public static PaperSize APlus => new PaperSize(227.Millimeters(), 356.Millimeters());

        public static PaperSize BPlus => new PaperSize(305.Millimeters(), 487.Millimeters());

        public static PaperSize LetterPlus => new PaperSize(8.5.Inches(), 12.69.Inches());

        public static PaperSize A4Plus => new PaperSize(210.Millimeters(), 330.Millimeters());

        public static PaperSize A5Transverse => new PaperSize(148.Millimeters(), 210.Millimeters());

        public static PaperSize B5Transverse => new PaperSize(182.Millimeters(), 257.Millimeters());

        public static PaperSize A3Extra => new PaperSize(322.Millimeters(), 445.Millimeters());

        public static PaperSize A5Extra => new PaperSize(174.Millimeters(), 235.Millimeters());

        public static PaperSize B5Extra => new PaperSize(201.Millimeters(), 276.Millimeters());

        public static PaperSize A2 => new PaperSize(420.Millimeters(), 594.Millimeters());

        public static PaperSize A3Transverse => new PaperSize(297.Millimeters(), 420.Millimeters());

        public static PaperSize A3ExtraTransverse => new PaperSize(322.Millimeters(), 445.Millimeters());

        public static PaperSize JapaneseDoublePostcard => new PaperSize(200.Millimeters(), 148.Millimeters());

        public static PaperSize A6 => new PaperSize(105.Millimeters(), 148.Millimeters());

        public static PaperSize LetterRotated => new PaperSize(11.Inches(), 8.5.Inches());

        public static PaperSize A3Rotated => new PaperSize(420.Millimeters(), 297.Millimeters());

        public static PaperSize A4Rotated => new PaperSize(297.Millimeters(), 210.Millimeters());

        public static PaperSize A5Rotated => new PaperSize(210.Millimeters(), 148.Millimeters());

        public static PaperSize B4JisRotated => new PaperSize(364.Millimeters(), 257.Millimeters());

        public static PaperSize B5JisRotated => new PaperSize(257.Millimeters(), 182.Millimeters());

        public static PaperSize JapanesePostcardRotated => new PaperSize(148.Millimeters(), 100.Millimeters());

        public static PaperSize JapaneseDoublePostcardRotated => new PaperSize(148.Millimeters(), 200.Millimeters());

        public static PaperSize A6Rotated => new PaperSize(148.Millimeters(), 105.Millimeters());

        public static PaperSize B6Jis => new PaperSize(128.Millimeters(), 182.Millimeters());

        public static PaperSize B6JisRotated => new PaperSize(182.Millimeters(), 128.Millimeters());

        public static PaperSize Standard12X11 => new PaperSize(12.Inches(), 11.Inches());

        public static PaperSize Prc16K => new PaperSize(146.Millimeters(), 215.Millimeters());

        public static PaperSize Prc32K => new PaperSize(97.Millimeters(), 151.Millimeters());

        public static PaperSize Prc32KBig => new PaperSize(97.Millimeters(), 151.Millimeters());

        public static PaperSize PrcEnvelopeNumber1 => new PaperSize(102.Millimeters(), 165.Millimeters());

        public static PaperSize PrcEnvelopeNumber2 => new PaperSize(102.Millimeters(), 176.Millimeters());

        public static PaperSize PrcEnvelopeNumber3 => new PaperSize(125.Millimeters(), 176.Millimeters());

        public static PaperSize PrcEnvelopeNumber4 => new PaperSize(110.Millimeters(), 208.Millimeters());

        public static PaperSize PrcEnvelopeNumber5 => new PaperSize(110.Millimeters(), 220.Millimeters());

        public static PaperSize PrcEnvelopeNumber6 => new PaperSize(120.Millimeters(), 230.Millimeters());

        public static PaperSize PrcEnvelopeNumber7 => new PaperSize(160.Millimeters(), 230.Millimeters());

        public static PaperSize PrcEnvelopeNumber8 => new PaperSize(120.Millimeters(), 309.Millimeters());

        public static PaperSize PrcEnvelopeNumber9 => new PaperSize(229.Millimeters(), 324.Millimeters());

        public static PaperSize PrcEnvelopeNumber10 => new PaperSize(324.Millimeters(), 458.Millimeters());

        public static PaperSize Prc16KRotated => new PaperSize(146.Millimeters(), 215.Millimeters());

        public static PaperSize Prc32KRotated => new PaperSize(97.Millimeters(), 151.Millimeters());

        public static PaperSize Prc32KBigRotated => new PaperSize(97.Millimeters(), 151.Millimeters());

        public static PaperSize PrcEnvelopeNumber1Rotated => new PaperSize(165.Millimeters(), 102.Millimeters());

        public static PaperSize PrcEnvelopeNumber2Rotated => new PaperSize(176.Millimeters(), 102.Millimeters());

        public static PaperSize PrcEnvelopeNumber3Rotated => new PaperSize(176.Millimeters(), 125.Millimeters());

        public static PaperSize PrcEnvelopeNumber4Rotated => new PaperSize(208.Millimeters(), 110.Millimeters());

        public static PaperSize PrcEnvelopeNumber5Rotated => new PaperSize(220.Millimeters(), 110.Millimeters());

        public static PaperSize PrcEnvelopeNumber6Rotated => new PaperSize(230.Millimeters(), 120.Millimeters());

        public static PaperSize PrcEnvelopeNumber7Rotated => new PaperSize(230.Millimeters(), 160.Millimeters());

        public static PaperSize PrcEnvelopeNumber8Rotated => new PaperSize(309.Millimeters(), 120.Millimeters());

        public static PaperSize PrcEnvelopeNumber9Rotated => new PaperSize(324.Millimeters(), 229.Millimeters());

        public static PaperSize PrcEnvelopeNumber10Rotated => new PaperSize(458.Millimeters(), 324.Millimeters());

        public string Width => _width.SettingString;

        public string Height => _height.SettingString;
    }
}