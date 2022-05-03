namespace MyTaskManager.Models
{
    public class ToDoItem
    {
        /// <summary>
        /// Unique ID of the task
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the task
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// True if the task is completed
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// A recurring task
        /// </summary>
        public bool IsRecurring { get; set; }

        /// <summary>
        /// How often the task should be repeated
        /// </summary>
        public int? DaysToRecur { get; set; }
        
        /// <summary>
        /// If the date is movable, after completion the task will
        /// be moved by <see cref="DaysToRecur"/>.
        /// ex. Cleaning would be movable. Scheduled garbage day would not be
        /// </summary>
        public bool IsMoveableDate { get; set; }

        /// <summary>
        /// Due date for the ToDo
        /// </summary>
        public DateOnly DueDate { get; private set; }

        /// <summary>
        /// Sets the due date based on the <see cref="DaysToRecur"/>
        /// being added to the current date
        /// </summary>
        public void SetDueDate()
        {
            if (DaysToRecur.HasValue)
            {
                DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(DaysToRecur.Value));
            }
        }

        /// <summary>
        /// Sets the <see cref="DueDate"/> to the specified date
        /// </summary>
        /// <param name="dueDate">The new due date</param>
        public void SetDueDate(DateOnly dueDate)
        {
            DueDate = dueDate;
        }
    }
}
