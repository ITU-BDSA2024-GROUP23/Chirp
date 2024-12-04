public record UserInfoDTO
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required List<CheepDTO> Cheeps { get; set; }
    public required List<UserDTO> Following { get; set; }
    public required List<UserDTO> Followers { get; set; }
}
