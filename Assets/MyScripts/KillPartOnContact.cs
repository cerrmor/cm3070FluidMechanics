using UnityEngine;
using Obi;

[RequireComponent(typeof(ObiSolver))]
public class KillPartOnContact : MonoBehaviour
{
    ObiSolver solver;

    //ObiSolver.ObiCollisionEventArgs collisionEvent;

    void Awake()
    {
        solver = GetComponent<ObiSolver>();
    }

    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }

    void Solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
    {
        //gets the list of colliders in the world
        var world = ObiColliderWorld.GetInstance();

        //Checks each the collision distence against the tag killer
        //if the collider is named killer sets the life span of a particle to 0
        foreach (Oni.Contact contact in e.contacts)
        {
            // this one is an actual collision:
            if (contact.distance < 0.01)
            {
                ObiColliderBase col = world.colliderHandles[contact.bodyB].owner;

                // if this collider is tagged as "killer":
                if (col != null && col.gameObject.CompareTag("killer"))
                {
                    // get the index of the particle involved in the contact:
                    int particleIndex = solver.simplices[contact.bodyA];

                    ObiSolver.ParticleInActor pa = solver.particleToActor[particleIndex];
                    ObiEmitter emitter = pa.actor as ObiEmitter;

                    if (emitter != null)
                        emitter.life[pa.indexInActor] = 0;
                }
            }
        }
    }
}