public record UserInfoDTO
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required List<CheepDTO> Cheeps { get; set; }
    public required List<User> Following { get; set; }
    public required List<User> Followers { get; set; }
}

