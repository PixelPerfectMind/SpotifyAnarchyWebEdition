namespace SpotifyAnarchyWebEdition.Models
{
    public class Device
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrivateSession { get; set; }
        public bool IsRestricted { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int VolumePercent { get; set; }
        public bool SupportsVolume { get; set; }

        public string GetIconURL()
        {
            switch (Type)
            {
                case "Computer":
                    return "/Content/Images/DeviceTypes/computer.png";
                case "Smartphone":
                    return "/Content/Images/DeviceTypes/smartphone.png";
                case "Speaker":
                    return "/Content/Images/DeviceTypes/speaker.png";
                case "TV":
                    return "/Content/Images/DeviceTypes/television.png";
                default:
                    return "/Content/Images/unknown_item.png";
            }
        }
    }
}