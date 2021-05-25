import axios from "axios";
import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import getAxios from "../AuthAxios";
import { useAuthContext } from "../AuthContext";
import useInterval from "../Hooks/UseInterval";

const Home = () => {
const [joke, setJoke] = useState({id:'', setup:'', punchline:'', likes:'', dislikes:''});
const [userLikedJokeStatus, setUserLikesJokeStatus] = useState('')
const {user} = useAuthContext();



useEffect(() =>{
    getRandomJoke()
}, [])

const getRandomJoke = async () =>{
    const {data} = await getAxios().get('/api/jokes/getrandomjoke');
    const {data: userLikedJokeStatus} = await axios.get(`/api/jokes/getuserjokestatus/${data.id}`);
    setJoke(data);
    setUserLikesJokeStatus(userLikedJokeStatus.status);

}

const updateCounts = async () => {
        const {id} = joke;
        if (!id) {
            return;
        }
        const {data} = await axios.get(`/api/jokes/getcounts/${id}`);
        setJoke({...joke, likes: data.likes, dislikes: data.dislikes});
    }

useInterval(updateCounts, 500);

const LikeJoke = async(like) =>{
    const {id} = joke;
    await getAxios().post('api/jokes/like', {jokeId: id, like})
    const {data:userLikedJokeStatus} = await axios.get(`api/jokes/getuserjokestatus/${id}`);
    setUserLikesJokeStatus(userLikedJokeStatus.status);
}
const {setup, punchline, likes, dislikes} = joke;
const canLike = userLikedJokeStatus !=='Liked' &&  userLikedJokeStatus !=='CanNoLongerInteract';
const canDislike = userLikedJokeStatus !== 'Disliked' && userLikedJokeStatus !== 'CanNoLongerInteract';
console.log(userLikedJokeStatus);

return(
    <div className="row">
    <div className="col-md-6 offset-md-3 card card-body bg-light">
        {setup && <div>
            <h4>{setup}</h4>
            <h4>{punchline}</h4>
            <div>
                {userLikedJokeStatus !== 'Unauthenticated' && <div>
                    <button disabled={!canLike} onClick={() => LikeJoke(true)}
                            className="btn btn-primary">Like
                    </button>
                    <button disabled={!canDislike} onClick={() => LikeJoke(false)}
                            className="btn btn-danger">Dislike
                    </button>
                </div>}
                {userLikedJokeStatus === 'Unauthenticated' && <div>
                    <Link to='/login'>Login to your account to like/dislike this joke</Link>
                </div>}
                <br/>
                <h4>Likes: {likes}</h4>
                <h4>Dislikes: {dislikes}</h4>
                <h4>
                    <button className='btn btn-link' onClick={() => window.location.reload()}>Refresh
                    </button>
                </h4>
            </div>
        </div>}
        {!setup && <h3>Loading...</h3>}
    </div>
</div>   
)
        }
export default Home;