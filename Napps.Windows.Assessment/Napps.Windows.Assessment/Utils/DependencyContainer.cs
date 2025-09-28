using Caliburn.Micro;

namespace Napps.Windows.Assessment.Utils
{
	public interface IDependencyContainer
	{
		T Get<T>();

		T Get<T, T1>(T1 arg);
	}

	public class DependencyContainer : IDependencyContainer
	{
		private readonly SimpleContainer container;

		public DependencyContainer(SimpleContainer container)
		{
			this.container = container;
		}

		public T Get<T>() => container.GetInstance<T>();

		public T Get<T, TArg>(TArg arg)
		{
			var childContainer = container.CreateChildContainer();

			childContainer.RegisterHandler(typeof(TArg), null, _ => arg);

			var instance = childContainer.GetInstance<T>();

			childContainer.UnregisterHandler(typeof(TArg), null);

			return instance;
		}
	}
}
