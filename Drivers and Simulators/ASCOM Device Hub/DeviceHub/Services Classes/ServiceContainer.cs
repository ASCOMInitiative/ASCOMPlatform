using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCOM.DeviceHub
{
	// The ServiceContainer allows new concrete service instances to be
	// created and returned. To always return the same instance, pass that instance when
	// registering the service. To return a different instance each time one is requested,
	// pass the type of the concrete implementation to AddService.

	//Examples:
					
	// Singleton( the same instance is returned each time a service reference is requested.
	// ServiceContainer.Instance.AddService<ISpeechService>( new SpeechService() );
	
	// Unique( a different, unique instance is returned each time a service reference is requested.
	// ServiceContainer.Instance.AddService<IVCurvePointService>( typeof(DummyFocusPointGenerator) );

	public class ServiceContainer
    {
		// The service container is a singleton.

		public static ServiceContainer Instance { get; private set; }

		static ServiceContainer()
		{
			Instance = new ServiceContainer();
		}

		//  The _serviceMap Key is the Type of the Interface that definces the service
		//  The _serviceMap Value is a Dictionary of variant objects.
		//  The Key of the variant item is a string with the variant's name.
		//  The Value of the Variant item is either an instance of the concrete class that needs to be returned or
		//		the type of the concrete class that needs to be instantiated and returned.

		private readonly Dictionary<Type, Dictionary<string, object>> _serviceMap;

		private ServiceContainer()
		{
			_serviceMap = new Dictionary<Type, Dictionary<string, object>>();
		}

		public void ClearAllServices()
		{
			_serviceMap.Clear();
		}

		public void AddService<TServiceContract>( TServiceContract implementation )
			where TServiceContract : class
		{
			lock ( this )
			{
				AddNewService<TServiceContract>( implementation, "" );
			}
		}

		public void AddService<TServiceContract>( Type implType )
			where TServiceContract : class
		{
			lock ( this )
			{
				AddNewService<TServiceContract>( implType, "" );
			}
		}

		public void AddService<TServiceContract>( TServiceContract implementation, string variantKey )
		{
			lock ( this )
			{
				Type mapKey = typeof( TServiceContract );

				if ( _serviceMap.ContainsKey( mapKey ) )
				{
					Dictionary<string, object> serviceVariants = _serviceMap[mapKey];
					serviceVariants[variantKey] = implementation;
				}
				else
				{
					AddNewService<TServiceContract>( implementation, variantKey );
				}
			}
		}

		public void AddService<TServiceContract>( Type implType, string variantKey )
		{
			lock ( this )
			{
				Type mapKey = typeof( TServiceContract );

				if ( _serviceMap.ContainsKey( mapKey ) )
				{
					Dictionary<string, object> serviceVariants = _serviceMap[mapKey];
					serviceVariants[variantKey] = implType;
				}
				else
				{
					AddNewService<TServiceContract>( implType, variantKey );
				}
			}
		}

		public TServiceContract GetService<TServiceContract>()
			where TServiceContract : class
		{
			return GetService<TServiceContract>( "" );
		}

		public TServiceContract GetService<TServiceContract>( string variant )
			where TServiceContract : class
		{
			object serviceObject = null;

			lock ( this )
			{
				Dictionary<string, object> variants;

				if ( _serviceMap.TryGetValue( typeof( TServiceContract ), out variants ) )
				{
					object temp = null;
					variants.TryGetValue( variant, out temp );

					// Figure out if the variant object is a an instance of a service or a type of the concrete service class.

					Type serviceType = temp as Type;

					if ( serviceType == null )
					{
						// The object was registered, so return that instance.

						serviceObject = temp;
					}
					else
					{
						// The concrete type of the service was registered so create an instance of that object, using
						// the parameterless constructor ane return that instance.

						serviceObject = Activator.CreateInstance( serviceType );
					}
				}
			}

			return serviceObject as TServiceContract;
		}

		private void AddNewService<TServiceContract>( TServiceContract implementation, string variantKey )
		{
			// NOTE:  The container must have already been locked by the caller.
			// NOTE:  Any existing entry for this TServiceContract is overwritten!!!

			Dictionary<string, object> serviceVariants = new Dictionary<string, object>
			{
				[variantKey] = implementation
			};

			_serviceMap[typeof( TServiceContract )] = serviceVariants;
		}

		private void AddNewService<TServiceContract>( Type implType, string variantKey )
		{
			// NOTE:  The container must have already been locked by the caller.
			// NOTE:  Any existing entry for this TServiceContract is overwritten!!!

			// Verify that the specified type implements the service contract.

			Type svcType = typeof( TServiceContract );
			Type t = implType.GetInterface( svcType.Name );

			if ( t == null )
			{
				throw new ArgumentException( "Error in ServiceContainer.AddNewService: " + implType.GetType().Name + " does not implement " + svcType.Name + "." );
			}

			Dictionary<string, object> serviceVariants = new Dictionary<string, object>
			{
				[variantKey] = implType
			};

			_serviceMap[typeof( TServiceContract )] = serviceVariants;
		}
	}
}

