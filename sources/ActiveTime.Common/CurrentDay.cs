using System;

namespace DustInTheWind.ActiveTime.Common
{
    public class CurrentDay
    {
        private string comments;
        private DateTime date;

        public int? TimeRecordId { get; set; }

        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                comments = null;

                AreCommentsLoaded = false;
                AreCommentsSaved = true;
            }
        }

        public string Comments
        {
            get => comments;
            set
            {
                comments = value;
                AreCommentsSaved = false;
            }
        }

        public bool AreCommentsLoaded { get; private set; }

        public bool AreCommentsSaved { get; private set; }

        public CurrentDay()
        {
            AreCommentsSaved = true;
        }

        public void ResetComments(string newComments)
        {
            Comments = newComments;
            AreCommentsLoaded = true;
            AreCommentsSaved = true;
        }

        public void AcceptModifications()
        {
            AreCommentsSaved = true;
        }

        public void IncrementDate()
        {
            Date = Date.AddDays(1);
        }

        public void DecrementDate()
        {
            Date = Date.AddDays(-1);
        }
    }
}