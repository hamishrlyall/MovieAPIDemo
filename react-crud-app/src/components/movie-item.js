import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Col, Row } from 'react-bootstrap';
import logo from '../logo.svg';
//import withRouter from './withRouter'

const MovieItem = (props) => {

const redirect = useNavigate();

  return (
    <>
        <Row>
            <Col item xs={12} md={2}>
                <img src={props.data.coverImage || logo} style={{width:150, height:150}}></img>
            </Col>
            <Col item xs={12} md={10}>
                <div><b>{props.data.title}</b></div>
                <div>Actors: {props.data.actors.map( x => x.name).join(", ")}</div>
                <Button onClick={() => redirect('/details/' + props.data.id) }>See Details</Button>{' '}
                <Button onClick={() => redirect('/edit/' + props.data.id) }>Edit</Button>{' '}
                <Button variant="danger" onClick={() => props.deleteMovie(props.data.id)} danger>Delete</Button>
            </Col>
            <Col>
                <hr/>
            </Col>
        </Row>
    </>
  )

}

export default MovieItem;//withRouter( MovieItem );