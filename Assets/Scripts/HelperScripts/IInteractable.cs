public interface IInteractable
{
    void Interact(); //trigger interaction (defined in the item's script)
    string GetInteractPrompt(); 
}

/*
References:
[24] T. D. Wallace, Creating an Interaction System in Unity, 2023. [Online]. Available: https://tylerdwallace.com/creating-an-interaction-system-in-unity/

[25] “What do I need to use in order to make an interact method that acts differently for each object? Unity3D,” Stack Overflow, 2022. [Online]. 
    Available: https://stackoverflow.com/questions/72340017/what-do-i-need-to-use-in-order-to-make-an-interact-method-that-acts-differently
*/
