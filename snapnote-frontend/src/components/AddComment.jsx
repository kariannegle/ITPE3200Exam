import React, { useState } from 'react';

const AddComment = ({ postId, onAddComment }) => {
  const [newComment, setNewComment] = useState("");

  const handleAddComment = (e) => {
    e.preventDefault();

    if (newComment.trim() === "") {
      return;
    }

    const comment = {
      id: Date.now(), // Temporary ID
      author: 'Current User', // Replace with logged-in user details
      content: newComment,
      createdAt: new Date().toLocaleString()
    };

    setNewComment("");

    if (typeof onAddComment === 'function') {
      onAddComment(postId, comment);
    } else {
      console.error("onAddComment is not a function in AddComment!");
    }
  };

  return (
    <form onSubmit={handleAddComment} className="comment-form">
      <input
        type="text"
        value={newComment}
        onChange={(e) => setNewComment(e.target.value)}
        placeholder="Add a comment"
        className="form-control"
      />
      <button type="submit" className="submitcomment">Add Comment</button>
    </form>
  );
};

export default AddComment;