using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestMakerFreeWebApp.Data.Models
{
    public class Question
    {
        #region Constructor
        public Question() 
        { 

        }
        #endregion

        #region Properties
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int QuizId { get; set; }

        [Required]
        public string Text { get; set; }

        public string? Notes { get; set; }

        [DefaultValue(0)]
        public int Type { get; set; }

        [DefaultValue(0)]
        public int Flags { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastModifiedDate { get; set; }
        #endregion

        #region Properties lazily loaded
        /// <summary>
        /// Overarching quiz
        /// </summary>
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }

        /// <summary>
        /// List of answers linked to the question
        /// </summary>
        public virtual List<Answer> Answers { get; set; }
        #endregion
    }
}
