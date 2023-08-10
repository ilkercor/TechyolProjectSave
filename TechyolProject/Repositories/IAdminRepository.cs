namespace TechyolProject.Repositories
{
    public interface IAdminRepository
    {
        void AddItem(Product product);
        void SaveChanges();
    }
}