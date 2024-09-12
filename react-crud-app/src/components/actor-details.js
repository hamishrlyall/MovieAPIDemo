import React, { useEffect, useState } from "react";
import { Button, Col, Row } from "react-bootstrap";
import { Link, useParams } from "react-router-dom";

export const ActorDetail = () => {
  const [actor, setActor] = useState({});
  const { actorid } = useParams();

  useEffect(() => {
    fetch(`${process.env.REACT_APP_API_URL}/person/${actorid}`)
      .then((res) => res.json())
      .then((res) => {
        console.log("res", res);
        if (res.status === true) {
          setActor(res.data);
        }
      })
      .catch((err) => alert("Error in getting data"));
  }, []);

  return (
  <>
    <Row>
      {actor && 
      <>
      <Col item xs={12} md={12}>
        <h3>{actor.name}</h3>
        <div><b>Date Of Birth: </b></div>
        <div>{actor.dateOfBirth && actor.dateOfBirth.split('T')[0]}</div>
        <div><b>Movies: </b></div>
        <ul>{actor.movies?.map(x => <li>{x}</li>)}</ul>
      </Col>
      <Col>
        <Link to="/">Go to Home page</Link>
      </Col>
      </>}

    </Row>
  </>
  );
};

export default ActorDetail;
