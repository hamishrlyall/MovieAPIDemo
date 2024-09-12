import React, { useEffect, useState } from "react";
import ActorItem from "./actor-item";
import ReactPaginate from "react-paginate";
import { Button, Col, Row } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

export const ActorList = () => {
  const [actors, setActors] = useState(null);
  const [actorCount, setActorCount] = useState(0);
  const [page, setPage] = useState(0);
  const redirect = useNavigate();

  useEffect(() => {
    // get all movies
    getPerson();
  }, [page]);

  const getPerson = () => {
    fetch(
      process.env.REACT_APP_API_URL +
        "/person?pageSize=" +
        process.env.REACT_APP_PAGING_SIZE +
        "&pageIndex=" +
        page
    )
      .then((res) => res.json())
      .then((res) => {
        if (res.status === true && res.data.count > 0) {
          setActors(res.data.person);
          setActorCount(
            Math.ceil(res.data.count / process.env.REACT_APP_PAGING_SIZE)
          );
        }

        if (res.data.count === 0) {
          alert("There is no actor data in system");
        }
      })
      .catch((err) => alert("Error getting data"));
  };

  const handlePageClick = (pageIndex) => {
    setPage(pageIndex.selected);
  };

  const deletePerson = (id) => {
    fetch(process.env.REACT_APP_API_URL + "/person?id=" + id, {
      method: "DELETE",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
    })
      .then((res) => res.json())
      .then((res) => {
        if (res.status === true ) {
          alert(res.message);
          getPerson();
        }
      })
      .catch((err) => alert("Error getting data"));
  }

  return (
    <>
      {actors && actors.Count !== 0 ? 
      <div>
        {actors.map((m, i) => <Row key={i}>
          <Col>
            <div onClick={() => redirect('/actors/details/' + m.id)}><b><u>{m.name}</u></b></div>{' '}
            <Button onClick={() => redirect('/actors/create-edit/' + m.id ) }>Edit</Button>{' '}
            <Button variant="danger" onClick={() => m.deletePerson(m.id)} danger>Delete</Button>
            <hr/>
          </Col>
        </Row>)}
        </div>
        : ""}


      <div className="d-flex justify-content-center">
        <ReactPaginate
          previousLabel={"previous"}
          nextLabel={"next"}
          breakLabel={"..."}
          breakClassName={"page-link"}
          pageCount={actorCount}
          marginPagesDisplayed={2}
          pageReangeDisplayed={5}
          onPageChange={handlePageClick}
          containerClassName={"pagination"}
          pageClassName={"page-item"}
          pageLinkClassName={"page-link"}
          nextClassName={"active"}
        />
      </div>
    </>
  );
};

export default ActorList;
