namespace Lines.Scripts.Base.Api
{
    public interface IBaseRepository<out T>
    {
        public T ObtainDefault();
    }
}
