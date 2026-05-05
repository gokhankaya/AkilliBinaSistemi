namespace WebApi.Data;

public class AreaType
{
    public int ID { get; set; }
    public string? Name { get; set; }
    public string? Definition { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}
