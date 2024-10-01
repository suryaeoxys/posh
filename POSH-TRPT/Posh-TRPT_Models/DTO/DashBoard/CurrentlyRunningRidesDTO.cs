namespace Posh_TRPT_Models.DTO.DashBoard
{ 
    public class CurrentlyRunningRidesDTO
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string? PicUpAddress { get; set; }
        public string? DoAddress { get; set; }
        public string? Rider { get; set; }
        public string? Driver { get; set; }
        public string? Status { get; set; }
        public string? Category { get; set; }
        public string? NewDate { get; set; }
    }
}
