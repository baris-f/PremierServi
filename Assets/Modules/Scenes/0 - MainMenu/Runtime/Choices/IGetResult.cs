namespace Modules.Scenes._0___MainMenu.Runtime.Choices
{
    public interface IGetResult<out T>
    {
        public T GetResult(int id);
    }
}