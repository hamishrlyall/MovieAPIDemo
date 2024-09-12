import React, { useEffect, useState } from "react";
import { Col, Row } from "react-bootstrap";
import logo from "../logo.svg";
import { Link, useParams } from "react-router-dom";

const MovieDetail = () => {
  const [movie, setMovie] = useState({});
  const [actors, setActors] = useState({});
  const { movieid } = useParams();

  useEffect(() => {
    fetch(`${process.env.REACT_APP_API_URL}/movie/${movieid}`)
      .then((res) => res.json())
      .then((res) => {
        console.log("res", res);
        if (res.status === true) {
          setMovie(res.data);
        }
      })
      .catch((err) => alert("Error in getting data"));
  }, []);

  return (
  <>
    <Row>
      {movie && 
      <>
      <Col item xs={12} md={4}>
        <img src={movie.coverImage || logo } style={{width: 300, height: 300}}/>
      </Col>
      <Col item xs={12} md={8}>
        <h3>{movie.title}</h3>
        <p>{movie.description || "N/A"}</p>
        <div><b>Language: </b></div>
        <div>{movie.language}</div>
        <div><b>Release Date: </b></div>
        <div>{movie.releaseDate && movie.releaseDate.split('T')[0]}</div>
        <div><b>Cast: </b></div>
        <div>{movie.actors?.map(x => x.name).join(", ")}</div>
      </Col>
      <Col>
        <Link to="/">Go to Home page</Link>
      </Col>
      </>}

    </Row>
  </>
  );
};

export default MovieDetail;
