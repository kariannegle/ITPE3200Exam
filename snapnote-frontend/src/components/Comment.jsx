import React from 'react';
import PropTypes from 'prop-types';
import Dropdown from 'react-bootstrap/Dropdown';

const Comment = ({ comment, onDelete, onEdit }) => {
  const handleDelete = () => {
    if (typeof onDelete === 'function') {
      onDelete(comment.id);
    }
  };

  const handleEdit = () => {
    if (typeof onEdit === 'function') {
      onEdit(comment.id);
    }
  };

  return (
    <div className="discussion mb-3">
      <div className="d-flex justify-content-between align-items-center">
        <div className="d-flex align-items-center">
          <div className="userImg d-flex align-items-center">
            <a href="/" className="nav-link text-black">
              <img
                src="../assets/image.png"
                style={{ width: "25px", height: "25px" }}
              />
              <span className="username">{comment.author}</span>
            </a>
          </div>
          <div className="comment-date">
            <small>{new Date(comment.createdAt).toLocaleDateString()}</small>
          </div>
        </div>
        <Dropdown>
          <Dropdown.Toggle variant="secondary" id="dropdown-basic">
            Actions
          </Dropdown.Toggle>
          <Dropdown.Menu>
            <Dropdown.Item onClick={handleEdit}>Edit</Dropdown.Item>
            <Dropdown.Item onClick={handleDelete}>Delete</Dropdown.Item>
          </Dropdown.Menu>
        </Dropdown>
      </div>
      <div className="comment">
        <p>{comment.content}</p>
      </div>
    </div>
  );
};

Comment.propTypes = {
  comment: PropTypes.object.isRequired,
  handleEdit: PropTypes.func.isRequired,
  handleDelete: PropTypes.func.isRequired,
};

export default Comment;