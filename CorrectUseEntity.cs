using System.Collections.Generic;

namespace Sandbox.CorrectUseUtility
{
	public abstract class CorrectUseEntity : AnimatedEntity, IUse
	{
		public abstract bool IsUsable( Entity user );

		public abstract IEnumerable<string> GetUsableHitBoxTags();


		public virtual float MaxTargetDistance => 100f;
		
		public bool OnUse( Entity user )
		{
			if ( user is not Player player ) return OnUseNotPlayer( user );

			else return OnUsePlayer( player );



			


		}


		public virtual bool OnUsePlayer( Player user )
		{
			var eyePos = user.EyePosition;
			var eyeDir = user.EyeRotation.Forward;
			var eyeRot = Rotation.From( new Angles( 0.0f, user.EyeRotation.Yaw(), 0.0f ) );



			var tr = Trace.Ray( eyePos, eyePos + eyeDir * MaxTargetDistance )
			.UseHitboxes()
			.WithAnyTags( "solid", "player", "debris" )
			.Ignore( this )
			.Run();

			if ( !tr.Hit ) return false;

			var usablehitbox = GetUsableHitBoxTags();
			foreach ( var tag in usablehitbox )
			{
				if (tr.Hitbox.HasTag(tag))
				{
					return OnUsedHitBox( tag, user );
				}
			}

			return false;
			
		}


		public abstract bool OnUsedHitBox( string hitboxname, Player user );
			

		public virtual bool OnUseNotPlayer( Entity user ) 
		{
			return false;
		
		}


		public override void Spawn()
		{
			base.Spawn();

			EnableAllCollisions = true;
			PhysicsEnabled = true;
			UsePhysicsCollision = true;

			EnableHitboxes = true;
			Tags.Add( "solid" );
		}

	}
}
