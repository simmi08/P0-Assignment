namespace AssignmentP0.Model
{
    public class User
    {
        public string UserID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;

        public List<Availability>? Availabilities { get; set; }

    }
}
