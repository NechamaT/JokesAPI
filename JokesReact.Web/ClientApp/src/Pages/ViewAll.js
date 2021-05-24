import React, { useState, useEffect } from "react";
import getAxios from "../AuthAxios";
import JokeRow from "../components/JokeRow";

const ViewAll = () => {
  const [jokes, setJokes] = useState([]);
  useEffect(() => {
    const getJokes = async () => {
      const { data } = await getAxios().get("/api/jokes/getjokes");

      setJokes(data);
      console.log(jokes);
    };
    getJokes();
  }, []);

  return (
    <div className="row">
      <div className="col-md-6 offset-md-3">
        {jokes.map((j) => (
          <JokeRow joke={j} />
        ))}
      </div>
    </div>
  );
};

export default ViewAll;
