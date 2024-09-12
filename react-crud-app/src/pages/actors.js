import React, { useState } from "react";
import { Button, Col, Row } from "react-bootstrap";
import ActorList from "../components/actor-list";
import CreateActorModel from "../components/create-actor-model";

export const Actors = () => {
  const[show, setShow] = useState(false);

  return (
    <>
      <Row>
        <Col xs={12} md={10}>
          <h2>Actors</h2>
        </Col>
        <Col xs={12} md={2} classname="align-self-center">
          <Button classname="float-right" onClick={() => setShow(true)}>
            Add New Actor
          </Button>
        </Col>
      </Row>

      <ActorList />

      <CreateActorModel show={show} handleClose={() => setShow(false)}/>
    </>
  );
};

export default Actors;
