import React from "react";
import { Modal } from "react-bootstrap";
import CreateEditActor from "./create-edit-actor";

export const CreateActorModel = (props) => {
  return (
    <>
      <Modal
        show={props.show}
        onHide={props.handleClose}
        backdrop="static"
        keyboard={false}
        centered
      >
        <Modal.Header closeButton>
          <Modal.Title>Add New Actor</Modal.Title>
        </Modal.Header>
        
        <Modal.Body>
          <CreateEditActor />
        </Modal.Body>
      </Modal>
    </>
  );
};

export default CreateActorModel;
