import React, { useEffect, useState } from "react";
import { Button, Form } from "react-bootstrap";
import { useParams } from "react-router-dom";

const CreateEditActor = () => {
  const { personid } = useParams({});
  const [actor, setActor] = useState({});
  const [validated, setValidated] = useState(false);

  useEffect(() => {
    if (personid) {
      fetch(`${process.env.REACT_APP_API_URL}/person/${personid}`)
        .then((res) => res.json())
        .then((res) => {
          if (res.status === true) {
            let personData = res.data;
            if (
              personData.dateOfBirth !== null &&
              personData.dateOfBirth !== undefined
            ) {
              // removes time from releaseDate for use in front end
              personData.dateOfBirth = personData.dateOfBirth.split("T")[0];
            }
            setActor(personData);
          }
        })
        .catch((err) => alert("Error in getting data"));
    }
  }, [personid]);

  const handleSave = (event) => {
    event.preventDefault();
    const form = event.currentTarget;
    if (form.checkValidity() === false) {
      event.stopPropagation();
      setValidated(true);
      return;
    }

    if (actor && actor.id > 0) {
      // update
      fetch(process.env.REACT_APP_API_URL + "/person", {
        method: "PUT",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
        body: JSON.stringify(actor),
      })
        .then((res) => res.json())
        .then((res) => {
          if (res.status === true && res.data) {
            let personData = res.data;
            if (
              personData.dateOfBirth !== null &&
              personData.dateOfBirth !== undefined
            ) {
              // removes time from releaseDate for use in front end
              personData.dateOfBirth = personData.dateOfBirth.split("T")[0];
            }
            setActor(personData);
            alert("updated successfully.");
          }
        })
        .catch((err) => alert("Error getting data"));
    } else {
      //create
      fetch(process.env.REACT_APP_API_URL + "/person", {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
        },
        body: JSON.stringify(actor),
      })
        .then((res) => res.json())
        .then((res) => {
          if (res.status === true && res.data) {
            let personData = res.data;
            if (
              personData.dateOfBirth !== null &&
              personData.dateOfBirth !== undefined
            ) {
              // removes time from releaseDate for use in front end
              personData.dateOfBirth = personData.dateOfBirth.split("T")[0];
            }
            setActor(personData);
            alert("created successfully.");
          }
        })
        .catch((err) => alert("Error getting data"));
    }
  };

  const handleFieldChange = (event) => {
    var da = actor;
    da[event.target.name] = event.target.value;

    console.log("value", da);
    setActor((oldData) => {
      return { ...oldData, ...da };
    });
  };

  return (
    <>
      <Form noValidate validated={validated} onSubmit={handleSave}>
        <Form.Group controlId="formactorname">
          <Form.Label>Name</Form.Label>
          <Form.Control
            name="name"
            value={(actor && actor.name) || ""}
            required
            type="text"
            autocomplete="off"
            placeholder="Enter Actor Name"
            onChange={handleFieldChange}
          />
          <Form.Control.Feedback type="invalid">
            Please enter movie name.
          </Form.Control.Feedback>
        </Form.Group>
        <Form.Group controlId="formactordateofbirth">
          <Form.Label>Date of Birth</Form.Label>
          <Form.Control
            name="releaseDate"
            value={(actor && actor.dateOfBirth) || ""}
            required
            type="date"
            onChange={handleFieldChange}
          />
          <Form.Control.Feedback type="invalid">
            Please enter actors date of birth.
          </Form.Control.Feedback>
        </Form.Group>
        <Button type="submit">
          {actor && actor.id > 0 ? "Update" : "Create"}
        </Button>
      </Form>
    </>
  );
};

export default CreateEditActor;
