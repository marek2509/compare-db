using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Porównator_Baz.Entities
{
    [Table("PODMIOTY")]
    public class Podmioty
    {
        [Key]
        public int ID_ID { get; set; }
        public string TYP { get; set; }

        // ForeignKey for Osoby, Instytucje, Malzenstwa, Inne_Podm where TYP F, P, M, I
        public int ID_OS { get; set; }

        private Osoby _osoba;
        [ForeignKey("ID_OS")]
        public virtual Osoby OsobaFizyczna
        {
            get
            {
                return _osoba;
            }

            set
            {
                _osoba = TYP == "F" ? value : null;
            }
        }
        
        private Instytucje _instytucje;
        [ForeignKey("ID_OS")]
        public virtual Instytucje Instytucja
        {
            get
            {
                return _instytucje;
            }

            set
            {
                _instytucje = TYP == "P" ? value : null;
            }
        }

        private Malzenstwa _malzenstwa;
        [ForeignKey("ID_OS")]
        public virtual Malzenstwa Malzenstwo
        {
            get
            {
                return _malzenstwa;
            }

            set
            {
                _malzenstwa = TYP == "M" ? value : null;
            }
        }

        private InnePodmioty _innePodmioty;
        [ForeignKey("ID_OS")]
        public virtual InnePodmioty InnyPodmiot
        {
            get
            {
                return _innePodmioty;
            }

            set
            {
                _innePodmioty = TYP == "I" ? value : null;
            }
        }


    }
}