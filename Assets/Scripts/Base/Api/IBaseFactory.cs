namespace Lines.Scripts.Base.Api
{
    public interface IBaseFactory<out T>
    {
        public T Create();
    }
}
