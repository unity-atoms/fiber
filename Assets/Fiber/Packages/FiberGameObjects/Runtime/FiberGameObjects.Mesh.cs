using System;
using System.Collections.Generic;
using UnityEngine;
using Signals;

namespace Fiber.GameObjects
{
    public static partial class BaseComponentExtensions
    {
        public static MeshRendererComponent MeshRenderer(
            this BaseComponent component,
            SignalProp<List<Material>> materials = new(),
            SignalProp<Material> material = new()
        )
        {
            return new MeshRendererComponent(
                materials: materials,
                material: material
            );
        }
    }

    public class MeshRendererComponent : BaseComponent
    {
        public SignalProp<List<Material>> Materials { get; private set; }
        public SignalProp<Material> Material { get; private set; }

        public MeshRendererComponent(
            SignalProp<List<Material>> materials = new(),
            SignalProp<Material> material = new()
        ) : base()
        {
            Materials = materials;
            Material = material;

            if (!Materials.IsEmpty && !Materials.IsEmpty)
            {
                throw new ArgumentException("MeshRendererComponent: Cannot set both Materials and Material");
            }
        }

        public override VirtualBody Render()
        {
            var parentGO = F.GetParentGameObject();
            var meshRenderer = parentGO.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = parentGO.AddComponent<MeshRenderer>();
            }

            if (!Materials.IsEmpty)
            {
                meshRenderer.materials = Materials.Get().ToArray();

                if (Materials.IsSignal)
                {
                    F.CreateEffect((materials) =>
                    {
                        meshRenderer.materials = materials.ToArray();
                        return null;
                    }, Materials.Signal);
                }
            }
            else if (!Material.IsEmpty)
            {
                meshRenderer.material = Material.Get();

                if (Material.IsSignal)
                {
                    F.CreateEffect((material) =>
                    {
                        meshRenderer.material = material;
                        return null;
                    }, Material.Signal);
                }
            }

            return VirtualBody.Empty;
        }
    }
}