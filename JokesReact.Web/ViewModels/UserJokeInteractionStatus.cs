using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokesReact.Web.ViewModels
{
    public enum UserJokeInteractionStatus
    {
        Unauthenticated, 
        Liked, 
        Disliked,
        NeverInteracted, 
        CanNoLongerInteract
    }
}
