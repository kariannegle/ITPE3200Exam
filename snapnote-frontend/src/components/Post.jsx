// src/components/Post.jsx
import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { Dropdown, Button } from 'react-bootstrap';

const Post = ({ post, onDelete, onEdit }) => {
  const [isEditing, setIsEditing] = useState(false);
  const [editedContent, setEditedContent] = useState(post.content);

  const handleDelete = () => {
    if (typeof onDelete === 'function') {
      onDelete(post.id);
    } else {
      console.error("onDelete is not a function in Post!");
    }
  };

  const handleEdit = () => {
    setIsEditing(true);
  };

  const handleSave = () => {
    if (typeof onEdit === 'function') {
      onEdit(post.id, editedContent);
      setIsEditing(false);
    } else {
      console.error("onEdit is not a function in Post!");
    }
  };

  const handleCancel = () => {
    setEditedContent(post.content);
    setIsEditing(false);
  };

  return (
    <div className="post">
      <div className="post-details-box">
        <div className="userinfo">
          <div className="userImg">
            <img src="default-user.png" alt="User" />
          </div>
          <span className="username">{post.author}</span>
        </div>
        <div className="post-header">
          {isEditing ? (
            <textarea
              value={editedContent}
              onChange={(e) => setEditedContent(e.target.value)}
            />
          ) : (
            <p>{post.content}</p>
          )}
          <Dropdown>
            <Dropdown.Toggle variant="secondary" id="dropdown-basic">
              Options
            </Dropdown.Toggle>

            <Dropdown.Menu>
              <Dropdown.Item onClick={handleEdit}>Edit</Dropdown.Item>
              <Dropdown.Item onClick={handleDelete}>Delete</Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
          {isEditing && (
            <div>
              <Button variant="primary" onClick={handleSave}>Save</Button>
              <Button variant="secondary" onClick={handleCancel}>Cancel</Button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

Post.propTypes = {
  post: PropTypes.object.isRequired,
  onDelete: PropTypes.func.isRequired,
  onEdit: PropTypes.func.isRequired,
};

export default Post;