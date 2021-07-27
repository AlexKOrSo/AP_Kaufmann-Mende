using HTMLTools;



namespace Classes
{
    ///<include file = 'ClassesDoc/CategorizedImage.xml' path = 'CategorizedImage/Member[@name="CategorizedImage"]/*' />
    public class CategorizedImage : Image, IHtmlable, IHtmldata
    {
        ///<include file = 'ClassesDoc/CategorizedImage.xml' path = 'CategorizedImage/Member[@name="PredictedLabel"]/*' />
        public string PredictedImageLabel { get; set; }

        ///<include file = 'ClassesDoc/CategorizedImage.xml' path = 'CategorizedImage/Member[@name="Score"]/*' />
        public float[] Score { get; set; }

        public string GetLabel() => PredictedImageLabel;


        public string GetFilePath() => Path;
        ///<include file = 'ClassesDoc/CategorizedImage.xml' path = 'CategorizedImage/Member[@name="GetHTMLData"]/*' />
        public IHtmldata GetHtmldata() => this;
        public CategorizedImage(string LabeledAs)
        {
            this.LabeledAs = LabeledAs;
        }
        public CategorizedImage() { }
    }




}
