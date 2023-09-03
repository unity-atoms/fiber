using Fiber;
using Fiber.UI;

public class DocsLogoComponent : BaseComponent
{
    public override VirtualNode Render()
    {
        return F.Typography(
            text: "fiber",
            type: TypographyType.Heading3
        );
    }
}