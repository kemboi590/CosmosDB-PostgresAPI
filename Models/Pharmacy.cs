using System;

namespace CosmosPostgresAPI.Models;

public class Pharmacy
{
    public int PharmacyId { get; set; }
    public required string PharmacyName { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public int ZipCode { get; set; }

}
