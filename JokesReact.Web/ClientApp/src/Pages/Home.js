import React, { useState, useEffect } from 'react';
import {Link} from 'react-router-dom';
import getAxios from '../AuthAxios';
import {useAuthContext} from '../AuthContext';

const Home = () =>{
    const [joke, setJoke] = useState({});
    const [isLoading, setIsLoading] = useState(true);
    const {user} =  useAuthContext();
    
    useEffect(() =>{
        getRandomJoke()
    }, [])

    const getRandomJoke = async () =>{
        const {data} = await getAxios().get('/api/jokes/getrandomjoke');
        setJoke(data);
        setIsLoading(false);

    }

    
    return (
        <div className="col-md-6 offset-md-3 card card-body bg-light">
            <div>
                <h4>{joke.setup}</h4>
                <h4>{joke.punchline}</h4>
                <div>
                    
                  

                        <div>
                            <button className="btn btn-primary">Like</button>
                            <button className="btn btn-danger">Dislike</button>
                        </div>
                    
                    <br />
                    <h4>Likes: {!!joke && joke.likes}</h4>
                    <h4>Dislikes: {!!joke && joke.dislikes}</h4>
                    <h4><button className="btn btn-link">Refresh</button></h4>
                </div>
            </div>
        </div>
    )

}
export default Home;