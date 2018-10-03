using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class ReflectionSystem : ComponentSystem
{

    private struct Group
    {
        public PlatformActivatorComponent Activator;
        public Transform Transform;
    }

    [Inject] EndFrameBarrier endFrameBarrier;

    protected override void OnUpdate()
    {
        //var commandBuffer = endFrameBarrier.CreateCommandBuffer();

        //var entities = GetEntities<Group>();
        //var results = new NativeArray<RaycastHit>(entities.Length, Allocator.Temp);
        //var commands = new NativeArray<RaycastCommand>(entities.Length, Allocator.Temp);
        //int index = 0;
        //foreach (var entity in entities)
        //{
        //    var origin = entity.Transform.position;
        //    var direction = entity.Transform.forward;
        //    var transform = entity.Transform;
        //    commands[index] = new RaycastCommand(origin, direction, entity.Activator.MaxActivationDistance);
        //    index++;
        //}

        //var handle = RaycastCommand.ScheduleBatch(commands, results, 1);
        //handle.Complete();

        //index = 0;
        //foreach (var entity in entities)
        //{
        //    var origin = entity.Transform.position;
        //    var direction = entity.Transform.forward;
        //    var ray = new Ray(origin, direction);
        //    var hit = results[index];
        //    var lightInstance = entity.Activator.LightInstance;
        //    var reflectionLightPrefab = entity.Activator.ReflectionLightPrefab;
        //    if (hit.collider != null)
        //    {
        //        if (hit.collider.name == "Reflector")
        //        {
        //            if (lightInstance == null)
        //            {
        //                entity.Activator.ShouldCreateLightInstance = true;
        //               // lightInstance = Object.Instantiate(reflectionLightPrefab, hit.point, hit.transform.rotation);
        //            }
        //            else
        //            {
        //                lightInstance.transform.position = hit.point;
        //            }
        //            var point = hit.point;
        //            var normal = hit.transform.forward;
        //            var reflection = ray.direction - 2 * (Vector3.Dot(ray.direction, normal)) * normal;
        //            var lookTowardsPos = point + reflection * 2F;
        //            lightInstance.transform.LookAt(lookTowardsPos);
        //            Debug.DrawRay(point, reflection);
        //        }
        //        else if (lightInstance != null)
        //        {
        //            entity.Activator.ShouldBeDestroyed = true;
        //            //Object.Destroy(lightInstance);
        //            entity.Activator.LightInstance = null;
        //        }
        //    }
        //    else if (lightInstance != null)
        //    {
        //        entity.Activator.ShouldBeDestroyed = true;
        //        Object.Destroy(lightInstance);
        //        entity.Activator.LightInstance = null;
        //    }
        //    index++;
        //}

       

        //results.Dispose();
        //commands.Dispose();
    }

}
