import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import AddComment from '../components/AddComment';
import Comment from '../components/Comment';
import Dropdown from 'react-bootstrap/Dropdown';

function Post({ post, onEdit, onAddComment, onDeleteComment, onDelete }) {
  const [isEditing, setIsEditing] = useState(false);
  const [editedContent, setEditedContent] = useState(post.content);
  const [comments, setComments] = useState(post.comments || []);
  const [posts, setPosts] = useState([]);
  const [error, setError] = useState(null);

  const fetchPosts = async () => {
    try {
      // Fetch posts from API or database
      const response = await fetch('/api/posts');
      const data = await response.json();
      setPosts(data);
    } catch (err) {
      setError('Failed to fetch posts');
    }
  };

  useEffect(() => {
    fetchPosts();
  }, []);

  const handleEdit = () => {
    if (typeof onEdit === 'function') {
      onEdit(post.id, editedContent);
      setIsEditing(false);
    } else {
      console.error("onEdit is not a function in Post!");
    }
  };

  const handleAddComment = (comment) => {
    if (typeof onAddComment === 'function') {
      onAddComment(post.id, comment);
      setComments([...comments, comment]);
    } else {
      console.error("onAddComment is not a function in Post!");
    }
  };

  const handleDeleteComment = (commentId) => {
    const updatedComments = comments.filter(comment => comment.id !== commentId);
    setComments(updatedComments);
    if (typeof onDeleteComment === 'function') {
      onDeleteComment(post.id, commentId);
    } else {
      console.error("onDeleteComment is not a function in Post!");
    }
  };

  const handleDelete = () => {
    if (typeof onDelete === 'function') {
      onDelete(post.id);
    } else {
      console.error("onDelete is not a function in Post!");
    }
  };

  const handleSave = () => {
    handleEdit();
  };

  return (
    <div className="post">
      {/* Post Content Section */}
      <div className="post-content">
          <p>{post.content}</p>
        </div>
      {/* Post Image Section */}
      {post.image && (
        <div className="imagecontent">
          <img src={URL.createObjectURL(post.image)} alt="Post Content" />
        </div>
      )}
      <div className="post-details-box">
        {/* User Information Section */}
        <div className="userinfo">
          <div className="userImg">
            <img src="default-user.png" alt="User" />
          </div>
          <span className="username">{post.author}</span>
        </div>

        {/* Post Header Section */}
        <div className="post-header">
          {/* Dropdown Menu for Post Options */}
          <Dropdown className="dropdown-container">
            <Dropdown.Toggle variant="secondary" id={`dropdown-${post.id}`} className="dropdown-toggle">
              <i className="fas fa-ellipsis-v"></i>
            </Dropdown.Toggle>

            <Dropdown.Menu>
              <Dropdown.Item onClick={handleEdit}>Edit</Dropdown.Item>
              <Dropdown.Item onClick={handleDelete}>Delete</Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
      {/* Comment Section */}
      <div className="commentsection">
        <h4>Comments</h4>
        <div className="comments-scrollable">
          {comments.map(comment => (
            <Comment key={comment.id} comment={comment} onDelete={handleDeleteComment} />
          ))}
        </div>
        <AddComment postId={post.id} onAddComment={handleAddComment} />
      </div>
    </div>
  );
}

Post.propTypes = {
  post: PropTypes.object.isRequired,
  onEdit: PropTypes.func.isRequired,
  onAddComment: PropTypes.func, // Optional callback for adding comments
  onDeleteComment: PropTypes.func, // Optional callback for deleting comments
  onDelete: PropTypes.func.isRequired,
};

export default Post;